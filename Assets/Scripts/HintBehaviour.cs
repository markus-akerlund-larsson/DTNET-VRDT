using System.Collections;
using System.Collections.Generic;
using Autohand;
using Autohand.Demo;
using UnityEngine;

public class HintBehaviour : MonoBehaviour
{
    public static bool startFirstTime;
    public static bool playAudioFirstTime;
    public AudioClip[] audioClip;
    public GameObject hintSkip;
    public GameObject lineSkip;
    public GameObject[] hints;
    AudioSource audioSource;
    public bool[] play;
    private void Awake()
    {
        if (!startFirstTime)
        {
            HintControllers[] hintControllers = FindObjectsOfType<HintControllers>();
            for (int i = 0; i < hintControllers.Length; i++)
            {
                hintControllers[i].EnableHint();
            }
            GetComponent<HandTriggerAreaEvents>().enabled = true;
            startFirstTime = true;
        }
        if (startFirstTime)
        {
            //HintControllers[] hintControllers = FindObjectsOfType<HintControllers>();
            //for (int i = 0; i < hintControllers.Length; i++)
            //{
            //    hintControllers[i].EnableHand();
            //}
        }
        audioSource = GetComponent<AudioSource>();
        PlayAudio(0);
    }
    private void Start()
    {
        startFirstTime = true;
    }
    public void PlayAudio(int index)
    {
        if (!playAudioFirstTime)
        {
            if (!play[index])
            {
                audioSource.Stop();
                audioSource.PlayOneShot(audioClip[index]);

                if (index == 4)
                    Invoke("ChangeAudio", 24f);
                play[index] = true;
            }
          
        }
    }

    public void EnableHands() 
    {
        HintControllers[] hintControllers = FindObjectsOfType<HintControllers>();
        for (int i = 0; i < hintControllers.Length; i++)
            hintControllers[i].EnableHand();
    }
    void ChangeAudio() 
    {
        audioSource.Stop();
        audioSource.PlayOneShot(audioClip[5]);
        for (int i = 0; i < hints.Length; i++)
        {
            if (i==0 || i==1)
            {
                hints[i].SetActive(true);
            }
            else
            {
                hints[i].SetActive(false);
            }
        }
      
    }

    public void EnnableMove() 
    {
        FindObjectOfType<XRHandPlayerControllerLink>().moveAxis = Common2DAxis.primaryAxis;
    }
    // Start is called before the first frame update


}
