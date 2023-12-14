using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NurseTabletRecord : MonoBehaviour
{
    public Text MainText;
    [SerializeField] public GameObject AudioHintButton;
    [SerializeField] private NurseTabletPopupManager PopupManager;
    [SerializeField] private ChecklistMechanic ChecklistManager;
    public Toggle checkbox;
    [SerializeField] private string explainationText;
    [SerializeField] private string[] answerText;
    [SerializeField] private int id;
    [SerializeField] private GameObject HintButton;
    private string questionText;
    private bool NoHints = false;

    public void Start()
    {
        
    }

    public void SetData(string _questionText, string[] _answerText, string _explainText)
    {
        questionText = _questionText;
        MainText.text = _questionText;
        answerText = _answerText;
        if(_explainText != null)
        {
            explainationText = _explainText;
        }
        else
        {
            NoHints = true;
            HintButton.SetActive(false);
        }
    }

    public void ShowAudioHint()
    {
        if(!NoHints)
            AudioHintButton.SetActive(true);
    }

    public void ShowExplaination()
    {
        if (!NoHints)
            PopupManager.ShowExplaination(explainationText);
    }

    public void CheckboxChecked()
    {
        if (checkbox.isOn)
        {
            if (ChecklistManager.Oncoming)
            {
                PopupManager.ShowQuestion(questionText, answerText, id);
            }
        }


        //if (ChecklistManager.indicate)
            //ChecklistManager.DisableIndicate();
    }
}
