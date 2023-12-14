using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;


public class ChecklistMechanic : MonoBehaviour
{
    [field: SerializeField] public bool Oncoming { set; get; }
    public int[] correctAnswers;
    [SerializeField] Toggle[] checkBoxes;
    [SerializeField] string scenarioName;
    [SerializeField] string scenarioTag;
    [SerializeField] GameObject ScorePanel;
    [SerializeField] Text ScoreText;
    public NurseTabletRecord[] TabletRecords;
    int[] givenAnswers;
    [HideInInspector]
    public bool indicate;
    Timer timer;
    public void Awake()
    {
        TabletRecords = GetComponentsInChildren<NurseTabletRecord>();
        ParseTheScenario();
        timer = GetComponent<Timer>();
    }

    void ParseTheScenario() //use with caution, erases all the answers gathered from the player
    {
        var langTag = PlayerPrefs.GetInt(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "StudyLanguage", 0) == 0 ? "" : "German";
        CSVParser Scenario = new CSVParser("Scenarios/" + scenarioName + "/NursingTablets" +langTag);
        int i = -1;
        correctAnswers = new int[Scenario.rowData.Count];
        givenAnswers = new int[Scenario.rowData.Count];
        if(Scenario.rowData[0].Length == 7) //normal scenario
        {
            Debug.Log("handling a normal scenario with hints and audiohints");
            foreach (string[] row in Scenario.rowData)
            {
                if (i != -1 && TabletRecords.Length > i)
                {
                    string[] _answers = new string[3];
                    int _correct = Random.Range(0, 3);
                    correctAnswers[i] = _correct;
                    for (int j = 0; j < _correct; j++)
                    {
                        _answers[j] = row[3 + j];
                    }
                    _answers[_correct] = row[2];
                    for (int j = _correct + 1; j <= 2; j++)
                    {
                        _answers[j] = row[2 + j];
                    }
                    TabletRecords[i].SetData(row[0], _answers, row[1]);
                }
                i++;
            }
        }
        else //no hint scenario
        {
            Debug.Log("handling a scenario with no hints or audiohints");
            foreach (string[] row in Scenario.rowData)
            {
                if (i != -1 && TabletRecords.Length > i)
                {
                    string[] _answers = new string[3];
                    int _correct = Random.Range(0, 3);
                    correctAnswers[i] = _correct;
                    for (int j = 0; j < _correct; j++)
                    {
                        _answers[j] = row[2 + j];
                    }
                    _answers[_correct] = row[1];
                    for (int j = _correct + 1; j <= 2; j++)
                    {
                        _answers[j] = row[1 + j];
                    }
                    TabletRecords[i].SetData(row[0], _answers, null);
                }
                i++;
            }
        }

    }

    public void SaveAnswer(int _id, int _answer)
    {
        givenAnswers[_id] = _answer;
        if(_answer == correctAnswers[_id])
        {
            //run code if the correct answer is given
        }
        else
        {
            //run code if the wrong answer is given
        }
        CheckCompletion();

        timer.TimerStart();
    }

    public void CheckCompletion() {
        foreach (Toggle _checkbox in checkBoxes)
        {
            if (!_checkbox.isOn)
                return;
            //check in the answer is correct and probably save the score somewhere
        }
        //Everything is done
        ShiftChangeFinishScreen _scfs = FindObjectOfType<ShiftChangeFinishScreen>(true);
        if(_scfs != null)
        {
            FindObjectOfType<PauseManager>().ShowMultiplayerOutro();
        }
        PlayerPrefs.SetInt(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + scenarioTag, 1);
        ScorePanel.SetActive(true);
        ScoreText.text = CalculateScore() + "/" + correctAnswers.Length;
    }

    private int CalculateScore()
    {
        int _score = 0;
        for(int i = 0; i < correctAnswers.Length; i++)
        {
            if (correctAnswers[i] == givenAnswers[i])
                _score++;
        }
        return _score;
    }
    public void EnableIndicate()
    {
        for (int i = 0; i < TabletRecords.Length; i++)
        {
            //if (!TabletRecords[i].checkbox.isOn)
                //TabletRecords[i].MainText.color = Color.green;
        }
        indicate = true;
    }
    public void DisableIndicate() 
    {
        //for (int i = 0; i < TabletRecords.Length; i++)
            //TabletRecords[i].MainText.color = Color.white;

        indicate = false;
    }

    public void Reset()
    {
        foreach (Toggle _checkbox in checkBoxes)
        {
            _checkbox.isOn = false;
        }
        ParseTheScenario();
    }

    void Update()
    {
        
    }
}
