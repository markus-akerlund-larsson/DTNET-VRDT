using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScenarioTaskSystem;
using UnityEngine.Events;

public class DoOnTrigger : MonoBehaviour
{
    [SerializeField] Collider Target;
    [SerializeField] UnityEvent OnEnter;
    private void OnTriggerEnter(Collider other)
    {
        if(other == Target)
        {
            OnEnter.Invoke();
        }
    }
}
