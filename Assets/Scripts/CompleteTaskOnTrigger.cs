using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScenarioTaskSystem;

public class CompleteTaskOnTrigger : MonoBehaviour
{
    [SerializeField] Collider Target;
    private void OnTriggerEnter(Collider other)
    {
        if(other == Target)
        {
            GetComponent<Task>().Complete();
        }
    }
}
