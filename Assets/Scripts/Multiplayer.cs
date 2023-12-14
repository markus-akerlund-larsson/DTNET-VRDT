using System.Collections;
using System.Collections.Generic;
using Autohand;
using Autohand.Demo;
using Photon.Pun;
using UnityEngine;

public class Multiplayer : MonoBehaviour
{    
    AutoHandPlayer autoHandPlayer;
    PhotonView pv;
    Transform player;
    Transform playerHead;
    Transform playerHandRight;
    Transform playerHandLeft;
    [SerializeField] Transform multiplayerHead;
    [SerializeField] Transform multiplayerHandRight;
    [SerializeField] Transform multiplayerHandLeft;

    // Start is called before the first frame update
    void Start()
    {
        pv = GetComponent<PhotonView>();
        if (pv.IsMine)
        {
            autoHandPlayer = FindObjectOfType<AutoHandPlayer>();
            player = autoHandPlayer.gameObject.transform;
            playerHead = autoHandPlayer.headCamera.gameObject.transform;
            playerHandRight = autoHandPlayer.handRight.gameObject.transform;
            playerHandLeft = autoHandPlayer.handLeft.gameObject.transform;


            foreach (Transform child in multiplayerHead.GetComponentsInChildren<Transform>())
            {
                child.gameObject.layer = 27;
            }

            foreach (Transform child in multiplayerHandRight.GetComponentsInChildren<Transform>())
            {
                child.gameObject.layer = 27;
            }
            foreach (Transform child in multiplayerHandLeft.GetComponentsInChildren<Transform>())
            {
                child.gameObject.layer = 27;
            }
        }
         
    }
    void Update()
    {
        if (pv.IsMine)
        {
            transform.position = player.position;
            transform.rotation = player.rotation;

            multiplayerHead.position = playerHead.position;
            multiplayerHead.rotation = playerHead.rotation;

            multiplayerHandRight.position = playerHandRight.position;
            multiplayerHandRight.rotation = playerHandRight.rotation;

            multiplayerHandLeft.position = playerHandLeft.position;
            multiplayerHandLeft.rotation = playerHandLeft.rotation;
        }
        
    }
}
