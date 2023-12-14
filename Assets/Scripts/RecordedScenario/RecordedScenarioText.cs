using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Meta.WitAi.TTS.Utilities;
using System;
using Meta.WitAi.TTS.Integrations;
using Meta.WitAi.TTS;
using Meta.WitAi.TTS.Data;
using System.IO;
using GoogleTextToSpeech.Scripts;
using System.Security.Cryptography;

namespace RecordedScenario
{
    [System.Serializable]
    public struct Phrase
    {
        public Phrase(string _s, bool _h, int[] _tc, string[] _txt, AudioClip[] _voiceAudio)
        {
            Speaker = _s;
            Highlight = _h;
            Timecode = new int[_tc.Length];
            _tc.CopyTo(Timecode, 0);
            Text = new string[_txt.Length];
            _txt.CopyTo(Text, 0);
            VoiceAudio = new AudioClip[_voiceAudio.Length];
            _voiceAudio.CopyTo(VoiceAudio, 0);
        }
        public string Speaker;
        public bool Highlight;
        public int[] Timecode;
        public string[] Text;
        public AudioClip[] VoiceAudio;
    }

    [System.Serializable]
    public struct AnimationCall
    {
        public AnimationCall(string _t, int[] _tc)
        {
            AnimationTag = _t;
            Timecode = new int[_tc.Length];
            _tc.CopyTo(Timecode, 0);
        }
        public string AnimationTag;
        public int[] Timecode;
    }

    [System.Serializable]
    public class SpeakerRef
    {
        public string Name;
        public string[] LocalizedName;
        public TTSSpeaker Speaker;
        public GoogleTTS GoogleSpeaker;
        public void Speak(string _s)
        {
            Debug.Log("Speaker " + Name + " on gameobject " + Speaker.transform.parent.name + " -> " + Speaker.name);
            Speaker.Speak(_s);
        }
    }

    [System.Serializable]
    public class AnimationEntity
    {
        public string Name;
        public UnityEvent AnimationEvent;
        public void Activate()
        {
            AnimationEvent.Invoke();
        }
    }

    [ExecuteInEditMode]
    public class RecordedScenarioText : MonoBehaviour
    {
        [SerializeField] private string scenarioName;  
        public bool PlayOnAwake;
        public int PlayOnAwakeTimeout;
        public int ScenarioEndTimeout; //Timeout after the last phrase in the scenario is played
        public Action <string> speakAction;
        [SerializeField] private UnityEvent OnScenarioEnd;
        [SerializeField] private List<SpeakerRef> Speakers;
        [SerializeField] private List<AnimationEntity> AnimationEntities;
        [SerializeField] private Text TranscriptTextbox;
        private int previousTimeElapsed;
        private float TimeElapsed;
        private bool running;
        [SerializeField] private float TestScenarioSpeed = 1;
        [SerializeField] private List<Phrase> Phrases;
        [SerializeField] private List<AnimationCall> AnimationCalls;
        [SerializeField] private int language;

        // Start is called before the first frame update
        void Start()
        {
            if (PlayOnAwake)
                Invoke("Play", PlayOnAwakeTimeout);
            language = PlayerPrefs.GetInt(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "StudyLanguage", 0);
            //language = playerprefs...
        }

        #if UNITY_EDITOR
        public void InitiateSetup()
        {
            StopAllCoroutines();
            StartCoroutine("Setup");
        }

        public void BreakSetup()
        {
            StopAllCoroutines();
        }

        private IEnumerator Setup()
        {
            CSVParser Scenario = new CSVParser("Scenarios/" + scenarioName + "/RecordedScenarioText");
            int _len = Scenario.rowData[0].Length;
            int _langNumber = (_len - 2) / 2;
            Phrases = new List<Phrase>();
            AnimationCalls = new List<AnimationCall>();
            foreach (string[] _row in Scenario.rowData)
            {
                if (_row == Scenario.rowData[0])
                {
                    continue;
                }
                int[] _timecodes = new int[_langNumber];
                string[] _texts = new string[_langNumber];
                for (int i = 0; i < _langNumber; i++)
                {
                    _timecodes[i] = int.Parse(_row[2 + i * 2]);
                    _texts[i] = _row[3 + i * 2];
                }
                if (_row[0] == "ANIMATION")
                {
                    AnimationCalls.Add(new AnimationCall(_row[3], _timecodes));
                }
                else
                {
                    //preprocess English TTS audio
                    string _name = ComputeSHA256Hash(_row[0] + _texts[0]);
                    AudioClip[] _voiceAudio = new AudioClip[_langNumber];
                    SpeakerRef _speaker = Speakers.Find(a => a.Name == _row[0]);

                    _speaker.Speaker.runInEditMode = true;
                    _speaker.Speak(_texts[0]);
                    float _timeElapsedLog = 0;
                    while (_speaker.Speaker.IsLoading)
                    {
                        _timeElapsedLog += Time.deltaTime;
                        Debug.Log("Waiting for " + _speaker.Name + " to load " + _texts[0] + ". Time elapsed: " + _timeElapsedLog);
                        yield return 0;
                    }
                    _speaker.Speaker.AudioSource.Stop();

                    //preprocess the german stuff here;
                    _name = ComputeSHA256Hash(_row[0] + _texts[1]);
                    string _resourcesName = "Scenarios/" + scenarioName + "/TTS_AudioRecordings/German/" + _name;

                    //if one needs to change the naming conventions again
                    /*
                    string _fullName = Application.dataPath + "/Resources/" + _resourcesName;
                    string _oldname = ComputeSHA256Hash(_texts[1]);
                    string _oldresourcesName = "Scenarios/" + scenarioName + "/TTS_AudioRecordings/German/" + _oldname;
                    string _oldfullName = Application.dataPath + "/Resources/" + _oldresourcesName;
                    if (File.Exists(_oldfullName + ".wav"))
                    {
                        if(File.Exists(_fullName + ".wav"))
                        {
                            File.Delete(_fullName + ".wav");
                            File.Delete(_fullName + ".wav.meta");
                        }
                        File.Move(_oldfullName + ".wav", _fullName + ".wav");
                        File.Move(_oldfullName + ".wav.meta", _fullName + ".wav.meta");
                    }
                    else
                    {
                        Debug.Log("can't see " + _oldfullName);
                    }
                    */

                    _voiceAudio[1] = Resources.Load<AudioClip>(_resourcesName);
                    if(_voiceAudio[1] == null)
                    {
                        string _fullName = Application.dataPath + "/Resources/" + _resourcesName;
                        for (int i = 0; i < 3; i++)
                        {
                            _speaker.GoogleSpeaker.SaveToFile(_texts[1], _fullName);
                            while (_speaker.GoogleSpeaker.SaveWIP == 0)
                            {
                                Debug.Log("Waiting for " + _speaker.Name + " to load " + _texts[1] + ". Time elapsed: " + _timeElapsedLog);
                                yield return 0;
                            }
                            if (_speaker.GoogleSpeaker.SaveWIP == 1)
                                break;
                            else
                            {
                                if (i < 2)
                                    Debug.LogWarning("Couldn't load " + _name + ", trying " + (2 - i) + "more times");
                                else
                                    Debug.LogError("Did not manage to load " + _name + "after 3 attempts");
                            }
                        }
                        if (_speaker.GoogleSpeaker.SaveWIP == 1)
                            _voiceAudio[1] = Resources.Load<AudioClip>(_resourcesName);
                    }
                    Phrases.Add(new Phrase(_row[0], _row[1] != "0", _timecodes, _texts, _voiceAudio));
                }
                    
            }
        }
        #endif

        static string ComputeSHA256Hash(string input)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = sha256.ComputeHash(bytes);

                System.Text.StringBuilder builder = new System.Text.StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    builder.Append(hashBytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        private void ProcessTick(int _tickNumber)
        {
            foreach (Phrase _phrase in Phrases)
            {
                if (_phrase.Timecode[language] == _tickNumber)
                {
                    SpeakerRef _speaker = Speakers.Find(a => a.Name == _phrase.Speaker);
                    string textToAdd = "<b>" + _speaker.LocalizedName[language] + ":</b> " + _phrase.Text[language];
                    if (_phrase.Highlight)
                    {
                        textToAdd = "<i><color=#68FF9A>" + textToAdd + "</color></i>";
                    }
                    textToAdd += "\n";
                    TranscriptTextbox.text += textToAdd;
                    //_phrase.Speaker
                    if (_speaker != null)
                    {
                        if(language == 0)
                        {
                            _speaker.Speak(_phrase.Text[language]);
                            speakAction?.Invoke(_speaker.Name);
                        }
                        else
                        {
                            string _name = ComputeSHA256Hash(_phrase.Speaker + _phrase.Text[1]);
                            string _resourcesName = "Scenarios/" + scenarioName + "/TTS_AudioRecordings/German/" + _name;
                            AudioClip _toPlay = Resources.Load<AudioClip>(_resourcesName);
                            if (_toPlay != null)
                                _speaker.GoogleSpeaker.PlaySaved(_toPlay);
                            else
                                Debug.LogError("Phrase " + _phrase.Text[1] + " for " + _phrase.Speaker + " hasn't been prerecorded.");
                        }
                    }
                    else
                    {
                        Debug.LogWarning("No speaker found with name " + _phrase.Speaker + "!");
                    }
                    if (_phrase.Equals(Phrases[Phrases.Count - 1]))
                    {
                        Debug.Log("Scenario ended...");
                        Invoke("End", ScenarioEndTimeout);
                    }
                }
            }
            foreach (AnimationCall _call in AnimationCalls)
            {
                if (_call.Timecode[language] == _tickNumber)
                {
                    AnimationEntity _animation = AnimationEntities.Find(a => a.Name == _call.AnimationTag);
                    if (_animation != null)
                    {
                        _animation.Activate();
                    }
                    else
                    {
                        Debug.LogWarning("No animation found with tag " + _call.AnimationTag + "!");
                    }
                }
            }
        }

        private void End()
        {
            OnScenarioEnd.Invoke();
        }

        public void Play()
        {
            running = true;
        }

        public void Pause()
        {
            running = false;
        }

        private void Tick()
        {
            TimeElapsed += Time.deltaTime * TestScenarioSpeed;
            if(previousTimeElapsed < (int)TimeElapsed)
            {
                previousTimeElapsed = (int)TimeElapsed;
                ProcessTick((int)TimeElapsed);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (Application.isPlaying)
            {
                if (running)
                    Tick();
            }
        }
    }

}
