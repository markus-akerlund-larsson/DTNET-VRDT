using System.Collections;
using System.Collections.Generic;
using Autohand;
using ChatGPT_Patient;
using UnityEngine;

public class HandAreaTriggerController : MonoBehaviour
{
    public Vector3 scaleEnter;
    public Vector3 scaleExit;
    public InteractionHandler [] interactionHandler;
    // Start is called before the first frame update
    void Start()
    {
        scaleEnter = transform.localScale;
        interactionHandler = FindObjectsOfType<InteractionHandler>(true);
    }
    public void ChangeScaleTriggerArea(int state) 
    {
        if (state==0) 
            transform.localScale = scaleExit;

            
        if (state==1) 
        {
            for (int i = 0; i < interactionHandler.Length; i++)
            {
                if (interactionHandler[i].transform.parent.gameObject.activeSelf)
                    interactionHandler[i].Clear();
            }
            transform.localScale = scaleEnter;
        }
    }


}
