using System.Collections;
using System.Collections.Generic;
using Autohand;
using UnityEngine;

public class TabletDistanceGrabbableController : MonoBehaviour
{
    float grabbableDistance=0.05f;
    float defaultGrabbableDistance;
    // Start is called before the first frame update
    void Start()
    {
        SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
        sphereCollider.radius = 0.3f;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Hand>())
        {
            defaultGrabbableDistance = other.GetComponent<Hand>().reachDistance;
            other.GetComponent<Hand>().reachDistance = grabbableDistance;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<Hand>())
        {
            defaultGrabbableDistance = other.GetComponent<Hand>().reachDistance;
            other.GetComponent<Hand>().reachDistance = grabbableDistance;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Hand>())
        {
            other.GetComponent<Hand>().reachDistance = 0.25f;
        }
    }

}
