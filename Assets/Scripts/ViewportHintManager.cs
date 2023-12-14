using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewportHintManager : MonoBehaviour
{
    [SerializeField] GameObject[] Hints;
    [SerializeField] GameObject[] Icons;

    void Start()
    {
        
    }

    public void SwitchTo(int _id)
    {
        for (int i = 0; i < Hints.Length && i < Icons.Length; i++)
        {
            Hints[i].SetActive(i == _id);
            Icons[i].SetActive(i == _id);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
