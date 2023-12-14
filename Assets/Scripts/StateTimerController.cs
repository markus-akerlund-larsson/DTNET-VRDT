using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StateTimerController : MonoBehaviour
{
    public AudioSource audioSource;  
    public TextMeshPro timerText;
    [SerializeField] List<Animator> animationControllers = new List<Animator>();
    private Animator animator;
    [SerializeField] List<AnimationClip> animationClips = new List<AnimationClip>();   

    [SerializeField] List<AudioClip> audioClips = new List<AudioClip>();     
    [SerializeField] float audioLenght;
    [SerializeField] string currentPlayTime;
    public List<float> timerDelayAudio = new List<float>();
    public bool playaAudioNext;

    private List<float> animationsClipsLenght = new List<float>();
    private List<float> audioClipsLenght = new List<float>();
    private AudioClip audioClip;
    float _timeLeft;
    // Start is called before the first frame update
    void Start()
    {
        if (!audioSource)
        {
            if (GetComponent<AudioSource>())
                audioSource = GetComponent<AudioSource>();
            else
                audioSource = gameObject.AddComponent<AudioSource>();
        }

        for (int i = 0; i < animationClips.Count; i++)
            audioClipsLenght.Add(animationClips[i].length);

        for (int i = 0; i < audioClips.Count; i++)
            audioClipsLenght.Add(audioClips[i].length);

        PlayFirstAudio();
    }
    public void PlayWheelAnimation(int countPatient, float timerDelay)
    {
        animator = animationControllers[countPatient - 1];
        Invoke("PlayAnimation", timerDelay);
    }
    public void AddNewAudio(AudioClip audioClip) 
    {
        audioClips.Add(audioClip);
    }
    public void PlayAudioClip(AudioClip audioClip, float timerDelay)
    {
        this.audioClip = audioClip;

        audioLenght = this.audioClip.length;
        audioSource.clip = this.audioClip;

        Invoke("PlayAudio", timerDelay);
    }
    public void PlayAudioByIndex(int index)
    {
        if (audioClips.Count > index) 
        {
            this.audioClip = audioClips[index];
        }
        else
            return;

        this.audioClip = audioClips[0];

        audioLenght = this.audioClip.length;
        audioSource.clip = this.audioClip;

        Invoke("PlayAudio", timerDelayAudio[index]);
    }
    public void PlayFirstAudio()
    {
        if (audioClips[0])
            this.audioClip = audioClips[0];
        else
            return;
        
        this.audioClip = audioClips[0];

        audioLenght = this.audioClip.length;
        audioSource.clip = this.audioClip;

        Invoke("PlayAudio", timerDelayAudio[0]);
    }
    private IEnumerator StartTimer()
    {
        while (_timeLeft < audioLenght)
        {
            _timeLeft += Time.deltaTime;
            UpdateTimeText();
            yield return null;
        }
    }
    private void PlayAudio()
    {
        audioSource.Play();
        _timeLeft = 0;
        StartCoroutine(StartTimer());
    }

    private void PlayAnimation()
    {
        animator.SetTrigger("StandUp");
    }

    private void UpdateTimeText()
    {
        if (_timeLeft >= audioLenght) 
        {
            _timeLeft = audioLenght;
            if (playaAudioNext)
            {
                for (int i = 0; i < audioClips.Count; i++)
                {
                    if (audioClip= audioClips[i])
                        PlayAudioClip(audioClips[i + 1],timerDelayAudio[i+1]);
                }
            }
        }

        float minutes = Mathf.FloorToInt(_timeLeft / 60);
        float seconds = Mathf.FloorToInt(_timeLeft % 60);

        currentPlayTime= string.Format("{0:00} : {1:00}", minutes, seconds);
        if (timerText)
            timerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }
    // Update is called once per frame

}
