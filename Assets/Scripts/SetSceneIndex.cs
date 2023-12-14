using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetSceneIndex : MonoBehaviour
{
    // Start is called before the first frame update
    void OnDestroy()
    {
        SpawnLobby.nameScene = SceneManager.GetActiveScene().name;
        //if (!SceneManager.GetActiveScene().name.Contains("Lobby"))
        //{
        //    SpawnLobby.nameScene = SceneManager.GetActiveScene().name;
        //    Debug.Log("Scene index: " + SpawnLobby.indexScene);
        //}
    }
}
