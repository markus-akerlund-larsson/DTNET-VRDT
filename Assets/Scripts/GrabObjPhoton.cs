using System.Collections;
using System.Collections.Generic;
using Autohand;
using Photon.Pun;
using UnityEngine;

public class GrabObjPhoton : MonoBehaviour
{
    PhotonView pv;
    Grabbable grabbable;
    void Start()
    {
        pv = GetComponent<PhotonView>();
        grabbable = GetComponent<Grabbable>();
        grabbable.onHighlight.AddListener(ChangeOwner);
        grabbable.onGrab.AddListener(OnGrab);
        grabbable.onRelease.AddListener(OnRelease);
    }

    public void ChangeOwner(Hand hand, Grabbable grabbable)
    {
        pv.TransferOwnership(PhotonNetwork.LocalPlayer);
    }

    public void OnGrab(Hand hand, Grabbable grabbable)
    {
        if (pv.IsMine)
            pv.RPC("SetKinematicTrue", RpcTarget.All);
     
    }
    public void OnRelease(Hand hand, Grabbable grabbable)
    {
        if (pv.IsMine)
            pv.RPC("SetKinematicFalse", RpcTarget.All);
    }

    [PunRPC]
    void SetKinematicTrue()
    {
        if (!pv.IsMine)
            GetComponent<Rigidbody>().isKinematic = true;
    }

    [PunRPC]
    void SetKinematicFalse()
    {
        if (!pv.IsMine)
            GetComponent<Rigidbody>().isKinematic = false;
    }
}
