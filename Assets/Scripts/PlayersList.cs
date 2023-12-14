using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayersList : MonoBehaviour
{
    int playersCount;
    public List<GameObject> showCase;
    public Text textRole;
    public List<PlayerInfo> playersList = new List<PlayerInfo>();
    public List<PlayerInfo> playersReady = new List<PlayerInfo>();
    PhotonView pv;
    int randomRole;
    int viewIdPlayer1;
    int viewIdPlayer2;
    bool setRoles;
    bool forceSet;
    public UnityEvent onJoin;
    public UnityEvent onLeft;

    private void Start()
    {
        pv = GetComponent<PhotonView>();
        onJoin.AddListener(OnJoin);
    }

    public void ForceSetRoles()
    {
        forceSet = true;
    }

    void OnJoin() 
    {
        var playersInfo = FindObjectsOfType<PlayerInfo>();
        playersList.Clear();
        playersReady.Clear();

        for (int i = 0; i < playersInfo.Length; i++) 
        {
            playersList.Add(playersInfo[i]);

            if (playersInfo[i].playerRole == PlayerInfo.PlayerRole.Player)
                playersReady.Add(playersList[i]);
        }

        if (PhotonNetwork.IsMasterClient && playersReady.Count == 2)
        {
            viewIdPlayer1 = playersReady[playersReady.Count - 1].GetComponent<PhotonView>().ViewID;
            viewIdPlayer2 = playersReady[playersReady.Count - 2].GetComponent<PhotonView>().ViewID;
            SetRoles();
        }

        playersCount = PhotonNetwork.PlayerList.Length;
    }
    void SetRoles()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            int randomRole = Random.Range(1, 3);
            pv.RPC("SetPlayerRole", RpcTarget.All, randomRole, viewIdPlayer1, viewIdPlayer2);
        }
    }

    public void RestartGame(bool sameRoles)
    {
        if (!sameRoles)
        {
            randomRole = (randomRole == 1) ? 2 : 1;
        }
        pv.RPC("SetPlayerRole", RpcTarget.All, randomRole, viewIdPlayer1, viewIdPlayer2);
        pv.RPC("ResetChecklists", RpcTarget.All);
    }

    [PunRPC]
    void ResetChecklists()
    {
        ChecklistMechanic[] Tablets = FindObjectsOfType<ChecklistMechanic>();
        foreach (ChecklistMechanic tablet in Tablets)
        {
            tablet.Reset();
        }
    }

    [PunRPC]
    void SetPlayerRole(int randomRole, int view1, int view2)
    {
        this.randomRole = randomRole;
        viewIdPlayer1 = view1;
        viewIdPlayer2 = view2;
        setRoles = true;
    }

    private void Update()
    {

        if (playersCount > PhotonNetwork.PlayerList.Length) //when some player leaves the room
        {
            playersList.Clear();
            playersReady.Clear();

            var playersInfo = FindObjectsOfType<PlayerInfo>();
            for (int i = 0; i < playersInfo.Length; i++)
            {
                playersList.Add(playersInfo[i]);

                if (playersInfo[i].playerRole == PlayerInfo.PlayerRole.Player)
                    playersReady.Add(playersList[i]);
            }

            playersCount = PhotonNetwork.PlayerList.Length;
        }


        if (forceSet)
        {
            viewIdPlayer1 = playersReady[playersReady.Count - 1].GetComponent<PhotonView>().ViewID;
            if (!forceSet)
                viewIdPlayer2 = playersReady[playersReady.Count - 2].GetComponent<PhotonView>().ViewID;
            SetRoles();
            forceSet = false;
        }

        if (setRoles)
        {
            if (randomRole == 1)
            {
                for (int i = 0; i < playersList.Count; i++)
                {
                    if (playersList[i].GetComponent<PhotonView>().ViewID == viewIdPlayer1)
                        playersList[i].playerRole = PlayerInfo.PlayerRole.OffGoing;
                    if (playersList[i].GetComponent<PhotonView>().ViewID == viewIdPlayer2)
                        playersList[i].playerRole = PlayerInfo.PlayerRole.OnComing;
                }
            }
            if (randomRole == 2)
            {
                for (int i = 0; i < playersList.Count; i++)
                {
                    if (playersList[i].GetComponent<PhotonView>().ViewID == viewIdPlayer1)
                        playersList[i].playerRole = PlayerInfo.PlayerRole.OnComing;
                    if (playersList[i].GetComponent<PhotonView>().ViewID == viewIdPlayer2)
                        playersList[i].playerRole = PlayerInfo.PlayerRole.OffGoing;
                }
            }

            playersList[playersList.Count - 1].SetRoles();
            if(playersList.Count > 1)
                playersList[playersList.Count - 2].SetRoles();
            setRoles = false;
        }
    }

}
