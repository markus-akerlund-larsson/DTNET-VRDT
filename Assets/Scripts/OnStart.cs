using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnStart : MonoBehaviour
{
    [SerializeField] UnityEvent Event;
    void Start()
    {
        if(Event != null)
            Event.Invoke();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
