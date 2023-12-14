using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class ItemRoomPhoton : MonoBehaviour
{
    public Text roomName;
    public Text playerCount;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void SetInfo(RoomInfo info) 
    {
        roomName.text = info.Name;
        playerCount.text = info.PlayerCount + "/" + info.MaxPlayers;
    }

    public void JoinToListRoom() 
    {
        PhotonNetwork.JoinRoom(roomName.text);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
