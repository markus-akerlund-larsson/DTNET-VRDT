using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    //public float time;
    //[SerializeField] private Text timerText;
    ChecklistMechanic checklistMechanic;
    [SerializeField] float _timeLeft = 0f;
    private void Start()
    {
        checklistMechanic = GetComponent<ChecklistMechanic>();
        TimerStart();
    }
    private IEnumerator StartTimer()
    {
        while (_timeLeft > 0)
        {
            _timeLeft -= Time.deltaTime;
            UpdateTimeText();
            yield return null;
        }
    }

    public void TimerStart() 
    {
        _timeLeft = 40;
        StartCoroutine(StartTimer());
    }
    public void TimerStop()
    {
        StopAllCoroutines();
    }
    

    private void UpdateTimeText()
    {
        if (_timeLeft < 0 && !checklistMechanic.indicate) 
        {
            checklistMechanic.EnableIndicate();
            TimerStop();
            _timeLeft = 0;
        }

        float minutes = Mathf.FloorToInt(_timeLeft / 60);
        float seconds = Mathf.FloorToInt(_timeLeft % 60);
        //timerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);
    }
}
