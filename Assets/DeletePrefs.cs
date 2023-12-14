using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeletePrefs : MonoBehaviour
{

    [SerializeField]
    private bool deletePrefs = false;
    // Start is called before the first frame update
    void Start()
    {
        if (deletePrefs) PlayerPrefs.DeleteAll();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
