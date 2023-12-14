using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class SpawnPlayers : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject photonPlayer;
    [SerializeField] GameObject photonPlayerView;
    [SerializeField] List<Transform> spawnPoint;

    void Start()
    {
        int randPoint = Random.Range(0, spawnPoint.Count);

        if (!PhotonManager._viewerApp && PhotonManager.onlineMode)
        {
            PlayerInfo _PI = PhotonNetwork.Instantiate(photonPlayer.name, spawnPoint[randPoint].position, spawnPoint[randPoint].rotation).GetComponent<PlayerInfo>();
            if (player != null)
            {
                _PI.AutoHandPlayer = Instantiate(player, spawnPoint[randPoint].position, spawnPoint[randPoint].rotation);
            }
            
        }

        if (PhotonManager._viewerApp && PhotonManager.onlineMode) 
        {
            if (player != null)
                Instantiate(player, spawnPoint[randPoint].position, spawnPoint[randPoint].rotation);
            PhotonNetwork.Instantiate(photonPlayerView.name, spawnPoint[randPoint].position, spawnPoint[randPoint].rotation);
           
        }
           

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
