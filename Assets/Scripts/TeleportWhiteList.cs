using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportWhiteList : MonoBehaviour
{
    [SerializeField] GameObject [] teleportList;
    // Start is called before the first frame update
    void Start()
    {
        var ground = GameObject.FindGameObjectsWithTag("Ground");
        for (int i = 0; i < teleportList.Length; i++)
        {
            teleportList[i].layer = 25;
        }
        for (int i = 0; i < ground.Length; i++)
        {
            ground[i].layer = 25;
        }
    }

}
