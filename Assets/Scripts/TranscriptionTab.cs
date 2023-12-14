using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TranscriptionTab : MonoBehaviour
{
    TextMeshProUGUI message;
    // Start is called before the first frame update
    void Start()
    {
        message = GetComponent<TextMeshProUGUI>();
        SetMessage("speaker", "message");
    }
    public void SetMessage(string speaker, string message) 
    {
        this.message.text = speaker + " : " + message;
    }
}
