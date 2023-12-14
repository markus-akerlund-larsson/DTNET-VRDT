using System.Collections;
using System.Collections.Generic;
using Autohand;
using UnityEngine;

public class HandLayerController : MonoBehaviour
{
    Grabbable grabbable;
    // Start is called before the first frame update
    void Start()
    {
        grabbable = GetComponent<Grabbable>();
        grabbable.onGrab.AddListener(OnGrab);
        grabbable.onRelease.AddListener(OnRelease);
    }

    public void OnGrab(Hand hand, Grabbable grabbable) 
    {
        foreach (Renderer rend in hand.GetComponentsInChildren<Renderer>())
        {
            rend.gameObject.layer = 26;
        }
    
    }
    public void OnRelease(Hand hand, Grabbable grabbable)
    {
        foreach (Renderer rend in hand.GetComponentsInChildren<Renderer>())
        {
            rend.gameObject.layer = 29;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
