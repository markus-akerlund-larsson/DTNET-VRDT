using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Keyboard : MonoBehaviour
{
    public Text text;
    public string key;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnMouseDown()
    {
        if (key!="" && key!="enter")
            text.text = text.text + key;

        if (key == "")
            text.text = "";

        if (key == "enter")
            FindObjectOfType<JoinRoomController>().GetPassword();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
