using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MultipleCameraModes : MonoBehaviour
{
    [SerializeField] Camera mainCamera;
    ViewportHintManager HintManager;
    Camera Player1Camera;
    Camera Player2Camera;
    int mode = 0;

    void Start()
    {
        HintManager = FindObjectOfType<ViewportHintManager>();
    }

    void Update()
    {
        if (Player1Camera == null || Player2Camera == null)
        {
            PlayerInfo[] Players = FindObjectsOfType<PlayerInfo>();
            foreach(PlayerInfo _player in Players)
            {
                if(_player.playerRole == PlayerInfo.PlayerRole.OnComing)
                {
                    Player1Camera = _player.GetComponentInChildren<Camera>();
                }
                if(_player.playerRole == PlayerInfo.PlayerRole.OffGoing)
                {
                    Player2Camera = _player.GetComponentInChildren<Camera>();
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            if (Player1Camera == null)
            {
                mainCamera.enabled = true;
                mainCamera.rect = new Rect(Vector2.zero, new Vector2(1, 1));
                return;
            }
            mode += 1;
            mode = mode % 5;
            HintManager.SwitchTo(mode);
            switch (mode)
            {
                case (0):
                    Player1Camera.enabled = false;
                    if (Player2Camera != null)
                        Player2Camera.enabled = false;
                    mainCamera.enabled = true;
                    mainCamera.rect = new Rect(Vector2.zero, new Vector2(1, 1));
                    break;
                case (1):
                    Player1Camera.enabled = true;
                    Player1Camera.rect = new Rect(new Vector2(0.6f, 0.5f), new Vector2(0.4f, 0.5f));
                    if (Player2Camera != null)
                    {
                        Player2Camera.enabled = true;
                        Player2Camera.rect = new Rect(new Vector2(0.6f, 0f), new Vector2(0.4f, 0.5f));
                    }
                    mainCamera.enabled = true;
                    mainCamera.rect = new Rect(Vector2.zero, new Vector2(0.6f, 1));
                    break;
                case (2):
                    Player1Camera.enabled = true;
                    Player1Camera.rect = new Rect(new Vector2(0, 0), new Vector2(0.5f, 1));
                    if (Player2Camera != null)
                    {
                        Player2Camera.enabled = true;
                        Player2Camera.rect = new Rect(new Vector2(0.5f, 0), new Vector2(0.5f, 1));
                    }
                    mainCamera.enabled = false;
                    break;
                case (3):
                    Player1Camera.enabled = true;
                    Player1Camera.rect = new Rect(new Vector2(0, 0), new Vector2(1, 1));
                    if (Player2Camera != null)
                    {
                        Player2Camera.enabled = false;
                    }
                    break;
                case (4):
                    Player1Camera.enabled = false;
                    if (Player2Camera != null)
                    {
                        Player2Camera.enabled = true;
                        Player2Camera.rect = new Rect(new Vector2(0, 0), new Vector2(1, 1));
                    }
                    break;
            }
        }
    }
}
