using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignFiller : MonoBehaviour
{
    [SerializeField] string scenarioName;

    // Start is called before the first frame update
    void Start()
    {
        CSVParser SignData = new CSVParser("Scenarios/" + scenarioName + "/FloatingHints");
        ShowcaseSign[] Signs = FindObjectsOfType<ShowcaseSign>();

        foreach (ShowcaseSign _sign in Signs)
        {
            foreach (string[] data in SignData.rowData)
            {
                if(data[0] == _sign.Tag)
                {
                    _sign.Fill(data[1]);
                    break;
                }
            }
        }

        
    }
}
