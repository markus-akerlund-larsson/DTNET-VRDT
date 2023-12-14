using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class CodeLock : MonoBehaviourPunCallbacks
{
    private string code = "";
    PhotonManager photonManager;

    public Text showCode;
    public Text showFullError;
    [SerializeField] private int maxLength;
    private JoinRoomController MultiplayerController;

    private void Start()
    {
        photonManager = FindObjectOfType<PhotonManager>();
        MultiplayerController = GetComponent<JoinRoomController>();
    }
    public void AddDigit(string digit)
    {
        code += digit;
        if (code.Length > maxLength)
        {
            code = code.Substring(1, code.Length - 1);
        }
        showCode.text = code;
    }

    public void Backspace()
    {
        code = code.Substring(0, code.Length - 1);
        showCode.text = code;
    }

    public void Clear()
    {
        code = "";
        showCode.text = code;
    }

    public void Submit()
    {
        if (photonManager == null)
            return;
        if (code == "")
        {
            MultiplayerController.CreateRoom();
        }
        else
        {
            MultiplayerController.CheckPassword(code);
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        showFullError.text = message;
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        showFullError.text = message;
    }
}
