using System;
using GoogleTextToSpeech.Scripts.Data;
using UnityEngine;
using System.Collections.Generic;

namespace GoogleTextToSpeech.Scripts
{
    public class GoogleTTS : MonoBehaviour
    {
        [SerializeField] private VoiceScriptableObject voice;
        [SerializeField] private TextToSpeech textToSpeech;
        [SerializeField] private AudioSource audioSource;

        private Action<AudioClip> _audioClipReceived;
        private Action<BadRequestData> _errorReceived;
        [SerializeField] private Queue<AudioClip> audioQueue = new Queue<AudioClip>();
        [SerializeField] private Queue<string> textQueue = new Queue<string>();

        [SerializeField] private bool Logs = false;
		
		public int SaveWIP { private set; get; } = 1; // 0 for loading, 1 for free, 2 for error
        private string SaveLocation;

        public void SaveToFile(string _text, string _filename)
        {
            if (SaveWIP == 0)
            {
                Debug.LogError("Download is still in progress, but there was a call to make another request anyway.");
                return;
            }
            SaveWIP = 0;
            SaveLocation = _filename;
            _errorReceived += ErrorReceived;
            _audioClipReceived += SaveClip;
            textToSpeech.GetSpeechAudioFromGoogle(_text, voice, _audioClipReceived, _errorReceived);
		}

        // Add an AudioClip to the queue
        public void EnqueueAudioClip(AudioClip clip)
        {
            if (audioQueue.Contains(clip))
            {
                if(Logs)
                    Debug.Log("This thing just tried to feed me a duplicate");
            }
            else
            {
                audioQueue.Enqueue(clip);
                if (Logs)
                    Debug.Log("adding " + clip + " to the queue. There are now " + audioQueue.Count);
            }
            if (textQueue.Count > 0)
            {
                ProcessNextText();
            }
        }

        private void ProcessNextText()
        {
            if (Logs)
                Debug.Log("the text queue contains " + textQueue.Count + " entries");
            string textToSpeak = textQueue.Dequeue();
            _errorReceived += ErrorReceived;
            _audioClipReceived += EnqueueAudioClip;
            if (Logs)
                Debug.Log("sending " + textToSpeak + " to TTS");
            textToSpeech.GetSpeechAudioFromGoogle(textToSpeak, voice, _audioClipReceived, _errorReceived);
        }

        // Update is called once per frame
        void Update()
        {
            if (audioQueue.Count > 0 && !audioSource.isPlaying)
            {
                PlayNextAudioClip();
            }
        }

        // Play the next AudioClip in the queue
        private void PlayNextAudioClip()
        {
            AudioClip _clipToPlay = audioQueue.Dequeue();
            if (Logs)
                Debug.Log("playing clip with ID " + _clipToPlay.GetInstanceID().ToString() + ". \n The amount of audios left in the queue is " + audioQueue.Count);
            audioSource.PlayOneShot(_clipToPlay);
        }

        public void Speak(string _text)
        {
            _errorReceived += ErrorReceived;
            _audioClipReceived += AudioClipReceived;
            textToSpeech.GetSpeechAudioFromGoogle(_text, voice, _audioClipReceived, _errorReceived);
        }

        public void SpeakQueued(string[] _texts)
        {
            foreach (string _text in _texts)
            {
                if (_text == string.Empty)
                    continue;
                textQueue.Enqueue(_text);
            }
            ProcessNextText();
        }

        private void ErrorReceived(BadRequestData badRequestData)
        {
            Debug.Log($"Error {badRequestData.error.code} : {badRequestData.error.message}");
            SaveWIP = 2;
        }

        private void AudioClipReceived(AudioClip _clip)
        {
            audioSource.Stop();
            audioSource.clip = _clip;
            audioSource.Play();
        }

        private void SaveClip(AudioClip _clip)
        {
            SavWav.Save(SaveLocation, _clip);
            SaveWIP = 1;
        }

        public void PlaySaved(AudioClip _clip)
        {
            audioSource.Stop();
            audioSource.clip = _clip;
            audioSource.Play();
        }
    }
}
