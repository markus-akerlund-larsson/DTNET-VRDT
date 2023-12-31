using Autohand;
using Autohand.Demo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppManager : MonoBehaviour
{
    [SerializeField] private GameObject playerVR;
    [SerializeField] private GameObject playerVRRightHand;
    [SerializeField] private GameObject playerVRLeftHand;
    [SerializeField] private GameObject playerVRRightHandTeleport;
    [SerializeField] private GameObject playerVRLeftHandTeleport;
    [SerializeField] private GameObject GuideCanvas;
    [SerializeField] private GameObject ExamCanvas;

    [SerializeField] private List<AudioSource> audioSourceDialogues;
    [SerializeField] private List<AudioSource> audioSourceSounds;

    public AppSettingsVR appSettings;

    // Start is called before the first frame update
    void Start()
    {
        /*
        appSettings = FindObjectOfType<AppSettings>();
        if (appSettings!=null)
        {
            UpdateState();
        }
        */
        UpdateState();
    }


    public void UpdateState()
    {
        SetVolumeDialogue(PlayerPrefs.GetFloat(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "dialogueVolume"));
        SetVolumeSound(PlayerPrefs.GetFloat(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "soundVolume"));
        SetGuide(PlayerPrefs.GetInt(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "GuidedMode"));
    }

    public void SetVolumeDialogue(float volumeDialogue)
    {
        if (appSettings != null) for (int i = 0; i < audioSourceDialogues.Count; i++) audioSourceDialogues[i].volume = volumeDialogue;
    }
    public void SetVolumeSound(float volumeSound)
    {
        if (appSettings != null) for (int i = 0; i < audioSourceSounds.Count; i++) audioSourceSounds[i].volume = volumeSound;
    }

    public void SetGuide(int guide)
    {
        GuideCanvas.SetActive(guide == 1);
        ExamCanvas.SetActive(guide == 0);
    }
}
