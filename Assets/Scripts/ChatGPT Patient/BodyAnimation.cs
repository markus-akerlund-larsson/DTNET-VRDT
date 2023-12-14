using System.Collections;
using System.Collections.Generic;
using RecordedScenario;
using UnityEngine;

public class BodyAnimation : MonoBehaviour
{
    [SerializeField] Animator animator;
    RecordedScenarioText recordedScenarioText;
    public AudioSource audioSource;
    string speaker;
    bool playAudio;
    // Start is called before the first frame update
    void Start()
    {
        SetRandomSittingIdleAnimation();
        if (FindObjectOfType<RecordedScenarioText>())
            FindObjectOfType<RecordedScenarioText>().speakAction += StartSpeak;
        if (!audioSource && GetComponent<AudioSource>()) audioSource = GetComponent<AudioSource>();
    }
    public void SetBadMoodAnimation(string triggerName) 
    {
        animator.SetTrigger(triggerName);

        if (triggerName == null)
        {
            int rand = Random.Range(1, 4);
            switch (rand)
            {
                case 1:
                    animator.SetTrigger("BadMood1");
                    break;
                case 2:
                    animator.SetTrigger("BadMood2");
                    break;
                case 3:
                    animator.SetTrigger("BadMood3");
                    break;
                default:
                    break;
            }
        }
    }
    public void SetGoodMoodAnimation(string triggerName)
    {
        animator.SetTrigger(triggerName);

        if (triggerName == null)
        {
            int rand = Random.Range(1, 4);
            switch (rand)
            {
                case 1:
                    animator.SetTrigger("GoodMood1");
                    break;
                case 2:
                    animator.SetTrigger("GoodMood2");
                    break;
                case 3:
                    animator.SetTrigger("GoodMood3");
                    break;
                default:
                    break;
            }
        }
    }
    public void SetNeutrallAnimation(string triggerName)
    {
        animator.SetTrigger(triggerName);

        if (triggerName==null)
        {
            int rand = Random.Range(1, 4);
            switch (rand)
            {
                case 1:
                    animator.SetTrigger("NeutralMood1");
                    break;
                case 2:
                    animator.SetTrigger("NeutralMood2");
                    break;
                case 3:
                    animator.SetTrigger("NeutralMood3");
                    break;
                default:
                    break;
            }
        }
    }

    public void SetRandomSittingIdleAnimation() 
    {
        int rand = 0;
        if (animator.transform.parent.parent)
        {
            if (animator.transform.parent.parent.name.Contains("Male"))
                rand = Random.Range(1, 4);

            if (animator.transform.parent.parent.name.Contains("Female"))
                rand = Random.Range(1, 3);

        }
        
        switch (rand)
        {
            case 1:
                animator.SetTrigger("IdleSitting1");
                break;
            case 2:
                animator.SetTrigger("IdleSitting2");
                break;
            case 3:
                animator.SetTrigger("IdleSitting3");
                break;

            default:
                break;
        }
    }


    void StartSpeak(string speaker) 
    {
        if (speaker == "Nurse" && animator.transform.parent.parent.name.Contains("Female"))
        {
            animator.SetTrigger("StartTalking");
        }
        if (speaker == "Patient" && animator.transform.parent.parent.name.Contains("Male"))
        {
            animator.SetTrigger("StartTalking");
        }

        this.speaker = speaker;

    }


    void StopSpeak(string name)
    {
        int rand = 0;

        if (name == "Nurse" && animator.transform.parent.parent.name.Contains("Female"))
        {
            rand = Random.Range(1, 3);
        }
        if (name == "Patient" && animator.transform.parent.parent.name.Contains("Male"))
        {
            rand = Random.Range(1, 4);
        }

        switch (rand)
        {
            case 1:
                animator.SetTrigger("StopTalking1");
                break;
            case 2:
                animator.SetTrigger("StopTalking2");
                break;
            case 3:
                animator.SetTrigger("StopTalking3");
                break;

            default:
                break;
        }

    }
    public void SetStartTalkingAnimation()
    {
        animator.SetTrigger("StartTalking");
    }
    public void SetStopTalkingAnimation()
    {
        animator.SetTrigger("StopTalking");
        SetRandomSittingIdleAnimation();
    }

    private void Update()
    {
        if (audioSource.isPlaying && !playAudio)
        {
            playAudio = true;
        }
        if (!audioSource.isPlaying && playAudio)
        {
            StopSpeak(speaker);
            playAudio = false;
        }
    
    }
}
