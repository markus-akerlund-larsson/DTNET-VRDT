﻿using Autohand;
using Autohand.Demo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using Assets.SimpleLocalization;


public class UIController : MonoBehaviour
{
    [SerializeField] private Slider setDialogueVolumeStatus;
    [SerializeField] private Slider setSoundVolumeStatus;
    [SerializeField] private Slider setMusicVolumeStatus;
    [SerializeField] private Slider setWalkingSpeed;
    //[SerializeField] private Dropdown setLanguageStatus;
    //[SerializeField] private Dropdown setTeleportHandStatus;
    //[SerializeField] private Dropdown setMovetHandStatus;
    [SerializeField] private Toggle setSubstitlesStatus;
    [SerializeField] private Toggle setGuidesStatus;
    [SerializeField] private GameObject teleportChosen;
    [SerializeField] private GameObject smoothChosen;
    [SerializeField] private GameObject mixedChosen;
    [SerializeField] private GameObject englishChosen;
    [SerializeField] private GameObject germanChosen;
    [SerializeField] private GameObject lithuanianChosen;
    [SerializeField] private GameObject latvianChosen;
    [SerializeField] private GameObject swedishChosen;
    [SerializeField] private GameObject englishLearningChosen;
    [SerializeField] private GameObject germanLearningChosen;
    [SerializeField] private GameObject assistantChosen;
    [SerializeField] private GameObject nurseChosen;
    [SerializeField] private AudioMixer AppMixer;
    [SerializeField] private bool Realtime;
    [SerializeField] Button languageButton;
    public static float dialogueVolume;
    public static float soundVolume;
    public static float musicVolume;
    public static float walkingSpeed;
    public static string language;
    public static int learnedLanguage;
    public static string role;
    public static int teleport;
    public static int subtitles;
    public static int guides;
    public bool tutorial;
    //SceneLoader sceneLoader;
    void Start()
    {
        LoadSettingsIntoUI();
        //sceneLoader = FindObjectOfType<SceneLoader>();
        if (SceneManager.GetActiveScene().name == "Lobby")
            languageButton.interactable = true;
    }

    public void LoadSettingsIntoUI()
    {
        setDialogueVolumeStatus.value = PlayerPrefs.GetFloat(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "dialogueVolume", 0.5f);
        setSoundVolumeStatus.value = PlayerPrefs.GetFloat(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "soundVolume", 0.5f);
        setMusicVolumeStatus.value = PlayerPrefs.GetFloat(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "musicVolume", 0.5f);
        //setWalkingSpeed.value = PlayerPrefs.GetFloat("walkingSpeed", 1.5f);
        setSubstitlesStatus.isOn = PlayerPrefs.GetInt(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "Subtitles", 0) == 0;
        teleport = PlayerPrefs.GetInt(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "MovementType", 0);
        if (!tutorial) SetLocomotionType(teleport);
        learnedLanguage = PlayerPrefs.GetInt(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "StudyLanguage", 0);
        role = PlayerPrefs.GetString(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "Role", "Assistant");
        language = PlayerPrefs.GetString(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "Language", "English");
        LocalizationManager.Language = language;
        teleportChosen.SetActive(teleport == 0);
        smoothChosen.SetActive(teleport == 1);
        mixedChosen.SetActive(teleport == 2);
        englishLearningChosen.SetActive(learnedLanguage == 0);
        germanLearningChosen.SetActive(learnedLanguage == 1);
        englishChosen.SetActive(language == "English");
        germanChosen.SetActive(language == "German");
        lithuanianChosen.SetActive(language == "Lithuanian");
        latvianChosen.SetActive(language == "Latvian");
        swedishChosen.SetActive(language == "Swedish");
        assistantChosen.SetActive(role == "Assistant");
        nurseChosen.SetActive(role == "Nurse");
    }
    public void SetDialogueVolume() 
    {
        dialogueVolume = setDialogueVolumeStatus.value;
        PlayerPrefs.SetFloat(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "dialogueVolume", dialogueVolume);
        if (dialogueVolume == 0)
            AppMixer.SetFloat("Dialogues", -80);
        else
        {
            AppMixer.SetFloat("Dialogues", Mathf.Log(dialogueVolume) * 20);
        }
    }

    public void SetSoundVolume()
    {
        soundVolume = setSoundVolumeStatus.value;
        //appSettings.UpdateSettings();
        PlayerPrefs.SetFloat(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "soundVolume", soundVolume);
        if (soundVolume == 0)
            AppMixer.SetFloat("Sounds", -80);
        else
        {
            AppMixer.SetFloat("Sounds", Mathf.Log(soundVolume) * 20);
        }
    }

    public void SetMusicVolume()
    {
        musicVolume = setMusicVolumeStatus.value;
        //appSettings.UpdateSettings();
        PlayerPrefs.SetFloat(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "musicVolume", musicVolume);
        if (musicVolume == 0)
            AppMixer.SetFloat("Music", -80);
        else
        {
            AppMixer.SetFloat("Music", Mathf.Log(musicVolume) * 20);
        }
    }

    public void SetWalkingSpeed()
    {
        walkingSpeed = setWalkingSpeed.value;
        //appSettings.UpdateSettings();
        PlayerPrefs.SetFloat(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "walkingSpeed", walkingSpeed);
        FindObjectOfType<XRMovementControls>().SetMovementSpeed(walkingSpeed);
    }

    public void SetLanguage(int lang)
    {
        learnedLanguage = lang;
        PlayerPrefs.SetInt(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "StudyLanguage", lang);
        englishLearningChosen.SetActive(lang==0);
        germanLearningChosen.SetActive(lang==1);
    }

    public void SetLanguage(string lang)
    {
        language = lang;
        PlayerPrefs.SetString(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "Language", lang);
        LocalizationManager.Language = language;
        englishChosen.SetActive(language == "English");
        germanChosen.SetActive(language == "German");
        lithuanianChosen.SetActive(language == "Lithuanian");
        latvianChosen.SetActive(language == "Latvian");
        swedishChosen.SetActive(language == "Swedish");
    }

    public void SetLocomotionType(int LocomotionID)
    {
        //teleportLeftHand = setTeleportHandStatus.value;
        teleport = LocomotionID;
        teleportChosen.SetActive(teleport == 0);
        smoothChosen.SetActive(teleport == 1);
        PlayerPrefs.SetInt(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "MovementType", teleport);
        Debug.Log("Looking for a thing");
        //Object.FindObjectOfType<XRMovementControls>().SwitchLocomotion(teleport);
        Debug.Log("found one");
        if(Realtime)
            FindObjectOfType<XRMovementControls>().SwitchLocomotion(teleport);
    }
    public void SetHandType(int hand)
    {
        if (hand == 0) AutoHandPlayer.movementHand = MovementHand.Left;
        if (hand == 1) AutoHandPlayer.movementHand = MovementHand.Right;
    }

    public void SetGender(int genderID)
    {
        //teleportLeftHand = setTeleportHandStatus.value;
        PlayerPrefs.SetInt(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "Gender", genderID);
    }

    public void SetRole(string _role)
    {
        //teleportLeftHand = setTeleportHandStatus.value;
        PlayerPrefs.SetString(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "Role", _role);
    }

    public void SetSubtitles()
    {

        if (setSubstitlesStatus.isOn) 
            subtitles = 0;
        else
            subtitles = 1;
        PlayerPrefs.SetInt(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "Subtitles", subtitles);
    }

    public void SetGuides(bool guides)
    {
        PlayerPrefs.SetInt(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "GuidedMode", guides ? 1 : 0);
        
    }

    public void LoadScene(string name)
    {
        //if (sceneLoader != null) sceneLoader.LoadScene(name);
     
        //SceneManager.LoadScene(name);
    }

    public void Exit() 
    {
        Application.Quit();
    }
}

/*
 * using Autohand;
using Autohand.Demo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Audio;
using Assets.SimpleLocalization;


public class UIController : MonoBehaviour
{
    [SerializeField] private Slider setDialogueVolumeStatus;
    [SerializeField] private Slider setSoundVolumeStatus;
    [SerializeField] private Slider setMusicVolumeStatus;
    [SerializeField] private Slider setWalkingSpeed;
    //[SerializeField] private Dropdown setLanguageStatus;
    //[SerializeField] private Dropdown setTeleportHandStatus;
    //[SerializeField] private Dropdown setMovetHandStatus;
    [SerializeField] private Toggle setSubstitlesStatus;
    [SerializeField] private Toggle setGuidesStatus;
    [SerializeField] private GameObject teleportChosen;
    [SerializeField] private GameObject smoothChosen;
    [SerializeField] private GameObject mixedChosen;
    [SerializeField] private GameObject englishChosen;
    [SerializeField] private GameObject germanChosen;
    [SerializeField] private GameObject lithuanianChosen;
    [SerializeField] private GameObject latvianChosen;
    [SerializeField] private GameObject swedishChosen;
    [SerializeField] private GameObject assistantChosen;
    [SerializeField] private GameObject nurseChosen;
    [SerializeField] private AudioMixer AppMixer;
    [SerializeField] private bool Realtime;

    public static float dialogueVolume;
    public static float soundVolume;
    public static float musicVolume;
    public static float walkingSpeed;
    public static string language;
    public static string role;
    public static int teleport;
    public static int subtitles;
    public static int guides;
    //SceneLoader sceneLoader;
    void Start()
    {
        LoadSettingsIntoUI();
        //sceneLoader = FindObjectOfType<SceneLoader>();
    }

    public void LoadSettingsIntoUI()
    {
        setDialogueVolumeStatus.value = PlayerPrefs.GetFloat("dialogueVolume", 0.5f);
        setSoundVolumeStatus.value = PlayerPrefs.GetFloat("soundVolume", 0.5f);
        setMusicVolumeStatus.value = PlayerPrefs.GetFloat("musicVolume", 0.5f);
        //setWalkingSpeed.value = PlayerPrefs.GetFloat("walkingSpeed", 1.5f);
        setSubstitlesStatus.isOn = PlayerPrefs.GetInt("Subtitles", 0) == 0;
        teleport = PlayerPrefs.GetInt("MovementType", 0);
        language = PlayerPrefs.GetString("Language", "English");
        role = PlayerPrefs.GetString("Role", "Assistant");
        LocalizationManager.Language = language;
        teleportChosen.SetActive(teleport == 0);
        smoothChosen.SetActive(teleport == 1);
        mixedChosen.SetActive(teleport == 2);
        englishChosen.SetActive(language == "English");
        germanChosen.SetActive(language == "German");
        lithuanianChosen.SetActive(language == "Lithuanian");
        latvianChosen.SetActive(language == "Latvian");
        swedishChosen.SetActive(language == "Swedish");
        assistantChosen.SetActive(role == "Assistant");
        nurseChosen.SetActive(role == "Nurse");
    }
    public void SetDialogueVolume() 
    {
        dialogueVolume = setDialogueVolumeStatus.value;
        PlayerPrefs.SetFloat("dialogueVolume", dialogueVolume);
        if (dialogueVolume == 0)
            AppMixer.SetFloat("Dialogues", -80);
        else
        {
            AppMixer.SetFloat("Dialogues", Mathf.Log(dialogueVolume) * 20);
        }
    }

    public void SetSoundVolume()
    {
        soundVolume = setSoundVolumeStatus.value;
        //appSettings.UpdateSettings();
        PlayerPrefs.SetFloat("soundVolume", soundVolume);
        if (soundVolume == 0)
            AppMixer.SetFloat("Sounds", -80);
        else
        {
            AppMixer.SetFloat("Sounds", Mathf.Log(soundVolume) * 20);
        }
    }

    public void SetMusicVolume()
    {
        musicVolume = setMusicVolumeStatus.value;
        //appSettings.UpdateSettings();
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        if (musicVolume == 0)
            AppMixer.SetFloat("Music", -80);
        else
        {
            AppMixer.SetFloat("Music", Mathf.Log(musicVolume) * 20);
        }
    }

    public void SetWalkingSpeed()
    {
        walkingSpeed = setWalkingSpeed.value;
        //appSettings.UpdateSettings();
        PlayerPrefs.SetFloat("walkingSpeed", walkingSpeed);
        FindObjectOfType<XRMovementControls>().SetMovementSpeed(walkingSpeed);
    }

    public void SetLanguage(string lang)
    {
        language = lang;
        PlayerPrefs.SetString("Language", lang);
        LocalizationManager.Language = language;
        englishChosen.SetActive(language == "English");
        germanChosen.SetActive(language == "German");
        lithuanianChosen.SetActive(language == "Lithuanian");
        latvianChosen.SetActive(language == "Latvian");
        swedishChosen.SetActive(language == "Swedish");
    }

    public void SetLocomotionType(int LocomotionID)
    {
        //teleportLeftHand = setTeleportHandStatus.value;
        teleport = LocomotionID;
        teleportChosen.SetActive(teleport == 0);
        smoothChosen.SetActive(teleport == 1);
        PlayerPrefs.SetInt("MovementType", teleport);
        Debug.Log("Looking for a thing");
        //Object.FindObjectOfType<XRMovementControls>().SwitchLocomotion(teleport);
        Debug.Log("found one");
        if(Realtime)
            FindObjectOfType<XRMovementControls>().SwitchLocomotion(teleport);
    }
    public void SetHandType(int hand)
    {
        if (hand == 0) AutoHandPlayer.movementHand = MovementHand.Left;
    
        if (hand == 1) AutoHandPlayer.movementHand = MovementHand.Right;

    }

    public void SetGender(int genderID)
    {
        //teleportLeftHand = setTeleportHandStatus.value;
        PlayerPrefs.SetInt("Gender", genderID);
    }

    public void SetRole(string _role)
    {
        //teleportLeftHand = setTeleportHandStatus.value;
        PlayerPrefs.SetString("Role", _role);
    }

    public void SetSubtitles()
    {

        if (setSubstitlesStatus.isOn) 
            subtitles = 0;
        else
            subtitles = 1;
        PlayerPrefs.SetInt("Subtitles", subtitles);
    }

    public void SetGuides(bool guides)
    {
        PlayerPrefs.SetInt("GuidedMode", guides ? 1 : 0);
        
    }

    public void LoadScene(string name)
    {
        //if (sceneLoader != null) sceneLoader.LoadScene(name);
     
        //SceneManager.LoadScene(name);
    }

    public void Exit() 
    {
        Application.Quit();
    }
}
*/