using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PersistentSaveSystem
{
    public class CompleteRoom : MonoBehaviour
    {
        [SerializeField] string Tag;

        public void Complete()
        {
            Debug.Log("Complete");
            PlayerPrefs.SetInt(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + Tag, 1);
        }
    }

}
