using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class RoomManagerPhoton : MonoBehaviour
{
    PhotonManager photonManager;
    List<RoomInfo> roomInfo =>photonManager.roomInfo; 
    List<ItemRoomPhoton> roomList = new List<ItemRoomPhoton>();

    [SerializeField] ItemRoomPhoton itemPrefab;
   
    void Start()
    {
        photonManager = FindObjectOfType<PhotonManager>();
    }

    public  void UpdateList()
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            Destroy(roomList[i].gameObject);
        }

        for (int i = 0; i < roomInfo.Count; i++)
        {
            ItemRoomPhoton itemRoomPhoton = Instantiate(itemPrefab, gameObject.transform);
            roomList.Add(itemRoomPhoton);
            itemRoomPhoton.SetInfo(roomInfo[i]);
        }  
    }

}
