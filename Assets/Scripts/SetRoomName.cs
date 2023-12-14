using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetRoomName : MonoBehaviour
{
    Text textNameRoom;
    // Start is called before the first frame update
    void Start()
    {
        textNameRoom = GetComponent<Text>();
        textNameRoom.text = JoinRoomController.nameRoom;
    }

    
}
