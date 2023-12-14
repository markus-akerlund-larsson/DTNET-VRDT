using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PersistentSaveSystem
{
    public class ResetProgress : MonoBehaviour
    {
        [SerializeField] string[] LevelTags;
        public void DoAReset()
        {
            int _id = PlayerPrefs.GetInt("CurrentPlayerID", 0);
            foreach(string _tag in LevelTags)
            {
                PlayerPrefs.DeleteKey(_id + _tag);
            }
            
        }
    }

}
