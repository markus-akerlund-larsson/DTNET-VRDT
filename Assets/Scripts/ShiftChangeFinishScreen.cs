using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiftChangeFinishScreen : MonoBehaviour
{
    PlayersList PL;
    void Start()
    {
        PL = FindObjectOfType<PlayersList>();
    }

    public void Restart(bool sameRoles)
    {
        PL.RestartGame(sameRoles);
    }
}
