using System.Collections;
using System.Collections.Generic;
using Autohand;
using UnityEngine;

public class GrabController : MonoBehaviour
{
    public HipsEstimation hipsEstimation;
    public Transform startPosTablet;
    public Transform tablet;

    bool setStartPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void Update()
    {
        if (!setStartPos)
        {
            hipsEstimation.enabled = false;
            tablet.transform.position = startPosTablet.transform.position;
            tablet.transform.rotation = startPosTablet.transform.rotation;
            setStartPos = true;

        }
    }
 
    // Update is called once per frame
  
}
