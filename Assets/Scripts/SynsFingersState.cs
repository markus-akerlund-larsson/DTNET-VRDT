using System.Collections;
using System.Collections.Generic;
using Autohand;
using Autohand.Demo;
using Photon.Pun;
using UnityEngine;

public class SynsFingersState : MonoBehaviour
{

    [SerializeField] List<Finger> multiplayerFingersRight;
    [SerializeField] List<Finger> multiplayerFingersLeft;
    [SerializeField] XRControllerEvent gripRight, triggerRight, axisRight;
    [SerializeField] XRControllerEvent gripLeft, triggerLeft, axisLeft;
    PhotonView pv;
    Hand handRight;
    Hand handLeft;
    List<Finger> playerFingersRight = new List<Finger>();
    List<Finger> playerFingersLeft = new List<Finger>();
    

    void Start()
    {
        pv = GetComponent<PhotonView>();

        handRight = FindObjectOfType<AutoHandPlayer>().handRight;
        handLeft = FindObjectOfType<AutoHandPlayer>().handLeft;

        //for left hand
        gripLeft.link = handLeft.GetComponent<XRHandControllerLink>();
        triggerLeft.link = handLeft.GetComponent<XRHandControllerLink>();
        axisLeft.link = handLeft.GetComponent<XRHandControllerLink>();

        //for right hand
        gripRight.link = handRight.GetComponent<XRHandControllerLink>();
        triggerRight.link = handRight.GetComponent<XRHandControllerLink>();
        axisRight.link = handRight.GetComponent<XRHandControllerLink>();

        if (pv.IsMine)
        {
            for (int i = 0; i < handRight.fingers.Length; i++)
            {
                playerFingersRight.Add(handRight.fingers[i]);
            }
            for (int i = 0; i < handLeft.fingers.Length; i++)
            {
                playerFingersLeft.Add(handLeft.fingers[i]);
            }

            AddListener();
        }
    }

    public void AddListener()
    {
        gripLeft.Pressed.AddListener(() => WhaitSetFingerStateLeftHand());
        gripLeft.Released.AddListener(() => WhaitSetFingerStateLeftHand());
        triggerLeft.Pressed.AddListener(() => WhaitSetFingerStateLeftHand());
        triggerLeft.Released.AddListener(() => WhaitSetFingerStateLeftHand());
        axisLeft.Pressed.AddListener(() => WhaitSetFingerStateLeftHand());
        axisLeft.Released.AddListener(() => WhaitSetFingerStateLeftHand());

        gripRight.Pressed.AddListener(() => WhaitSetFingerStateRightHand());
        gripRight.Released.AddListener(() => WhaitSetFingerStateRightHand());
        triggerRight.Pressed.AddListener(() => WhaitSetFingerStateRightHand());
        triggerRight.Released.AddListener(() => WhaitSetFingerStateRightHand());
        axisRight.Pressed.AddListener(() => WhaitSetFingerStateRightHand());
        axisRight.Released.AddListener(() => WhaitSetFingerStateRightHand());
    }


    void WhaitSetFingerStateLeftHand()
    {
        Invoke("SetFingerStateLeftHand", 0.1f);
    }
    void SetFingerStateLeftHand()
    {
        for (int i = 0; i < playerFingersLeft.Count; i++)
        {
            multiplayerFingersLeft[i].SetFingerBend(playerFingersLeft[i].bendOffset);
        }
        pv.RPC("SetFingerStateLeftHandRPC", RpcTarget.All, pv.ViewID, playerFingersLeft[0].bendOffset, playerFingersLeft[1].bendOffset, playerFingersLeft[2].bendOffset, playerFingersLeft[3].bendOffset, playerFingersLeft[4].bendOffset);
    }

    [PunRPC]
    void SetFingerStateLeftHandRPC(int viewId, float finger0, float finger1, float finger2, float finger3, float finger4)
    {
        if (viewId == pv.GetComponent<PhotonView>().ViewID)
        {
            multiplayerFingersLeft[0].SetFingerBend(finger0);
            multiplayerFingersLeft[1].SetFingerBend(finger1);
            multiplayerFingersLeft[2].SetFingerBend(finger2);
            multiplayerFingersLeft[3].SetFingerBend(finger3);
            multiplayerFingersLeft[4].SetFingerBend(finger4);
        }
    }
    void WhaitSetFingerStateRightHand()
    {
        Invoke("SetFingerStateRightHand", 0.1f);
    }
    void SetFingerStateRightHand()
    {
        for (int i = 0; i < playerFingersRight.Count; i++)
        {
            multiplayerFingersRight[i].SetFingerBend(playerFingersRight[i].bendOffset);
        }
        pv.RPC("SetFingerStateRightHandRPC", RpcTarget.All, pv.ViewID, playerFingersRight[0].bendOffset, playerFingersRight[1].bendOffset, playerFingersRight[2].bendOffset, playerFingersRight[3].bendOffset, playerFingersRight[4].bendOffset);
    }

    [PunRPC]
    void SetFingerStateRightHandRPC(int viewId, float finger0, float finger1, float finger2, float finger3, float finger4)
    {
        pv = GetComponent<PhotonView>();

        if (viewId == pv.GetComponent<PhotonView>().ViewID)
        {
            multiplayerFingersRight[0].SetFingerBend(finger0);
            multiplayerFingersRight[1].SetFingerBend(finger1);
            multiplayerFingersRight[2].SetFingerBend(finger2);
            multiplayerFingersRight[3].SetFingerBend(finger3);
            multiplayerFingersRight[4].SetFingerBend(finger4);
        }
    }
    void Update()
    {

    }
}
