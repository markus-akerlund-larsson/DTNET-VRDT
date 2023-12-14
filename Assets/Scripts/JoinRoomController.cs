using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class JoinRoomController : MonoBehaviour
{
    PhotonManager photonManager;
    CodeLock codeLock;
    public static string nameRoom;
    List<RoomInfo> roomInfo => photonManager.roomInfo;

    void Start()
    {
        photonManager = FindObjectOfType<PhotonManager>();
        codeLock = GetComponent<CodeLock>();
    }

    public void CreateNameRoom() 
    {
        codeLock.showCode.text = Random.Range(1000, 9999).ToString();
        nameRoom = codeLock.showCode.text;
    }

    public void CreateRoom()
    {
        CreateNameRoom();
        Debug.Log("Create room " + codeLock.showCode.text);
        photonManager.CreateRoom(codeLock.showCode.text);
    }

    public void GetPassword() 
    {
        CheckPassword(codeLock.showCode.text);
    }

    public void CheckPassword(string _roomCode)
    {
        for (int i = 0; i < roomInfo.Count; i++)
        {
            if (roomInfo[i].Name == _roomCode)
            {
                nameRoom = _roomCode;
                PhotonNetwork.JoinRoom(_roomCode);
                return;
            }
        }
        codeLock.showCode.text = "Error!";
    }

}
