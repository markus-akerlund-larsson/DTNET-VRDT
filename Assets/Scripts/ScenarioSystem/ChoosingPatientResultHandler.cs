using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScenarioTaskSystem;
using UnityEngine.UI;
using UnityEngine.Events;

public class ChoosingPatientResultHandler : ResultHandler
{
    [SerializeField] GameObject Panel;
    [SerializeField] GameObject CorrectText;
    [SerializeField] GameObject IncorrectText;
    [SerializeField] UnityEvent CorrectAction;
    [SerializeField] UnityEvent IncorrectAction;
    
    public override void HandleResult(Scenario _scenario)
    {
        Panel.SetActive(true);
        bool _correct = _scenario.TotalScore() > 0;
        CorrectText.SetActive(_correct);
        IncorrectText.SetActive(!_correct);
        if (_correct)
        {
            CorrectAction.Invoke();
        }
        else
        {
            IncorrectAction.Invoke();
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}


