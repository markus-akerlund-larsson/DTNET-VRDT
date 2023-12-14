using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StateMachine : MonoBehaviour
{
    //not really a state machine, since any state goes into any other one but whatever
    //could not think of a better name + we have no other state machines in the proj anyway

    [System.Serializable]
    public class State{
        public UnityEvent[] Events;
        public UnityEvent OnPicked;
        public UnityEvent OnLeft;

        public void Fire(int _EventID)
        {
            if (Events.Length > _EventID)
            {
                Events[_EventID].Invoke();
                return;
            }
            Debug.LogError("No such event in that state");
        }

        public void Picked()
        {
            OnPicked.Invoke();
        }

        public void Left()
        {
            OnLeft.Invoke();
        }
    }

    [SerializeField] State[] States;
    private int CurrentState;
    
    public void SwitchState(int _State)
    {
        if(States.Length > _State)
        {
            States[CurrentState].Left();
            CurrentState = _State;
            States[CurrentState].Picked();
            return;
        }
        Debug.LogError("No such state in that machine");
    }

    public void Fire(int _EventID)
    {
        States[CurrentState].Fire(_EventID);
    }
}
