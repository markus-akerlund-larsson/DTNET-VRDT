using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Autohand.Demo;
using Autohand;
using UnityEngine.SceneManagement;

public class XRMovementControls : MonoBehaviour
{
    [SerializeField] XRHandPlayerControllerLink MovementControls;
    [SerializeField] AutoHandPlayer AHPlayer;
    [SerializeField] GameObject TeleportRight;
    [SerializeField] GameObject TeleportLeft;
    void Awake()
    {
        SwitchLocomotion(PlayerPrefs.GetInt(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "MovementType", 0));
        AHPlayer.maxMoveSpeed = PlayerPrefs.GetFloat(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "walkingSpeed", 2);
        if (SceneManager.GetActiveScene().name=="Lobby")
            SwitchLocomotion(2);
    }

    public void SwitchLocomotion(int type)
    {
        switch (type)
        {

            case (0):
                AutoHandPlayer.movementType= MovementType.Teleport;
                //Teleport.SetActive(true);
                break;
            case (1):
                AutoHandPlayer.movementType = MovementType.Move;
                //Teleport.SetActive(false);
                break;
            case (2):
                AutoHandPlayer.movementType = MovementType.Mixed;
                //Teleport.SetActive(true);
                break;
        }

    }
    public void SwitchMovementHand(int type)
    {
        switch (type)
        {

            case (0):
                AutoHandPlayer.movementHand = MovementHand.Right;
                TeleportRight.SetActive(true);
                TeleportLeft.SetActive(false);
                break;
            case (1):
                AutoHandPlayer.movementHand = MovementHand.Left;
                TeleportRight.SetActive(false);
                TeleportLeft.SetActive(true);
                break;
           
        }

    }

    public void SetMovementSpeed(float speed)
    {
        AHPlayer.maxMoveSpeed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
