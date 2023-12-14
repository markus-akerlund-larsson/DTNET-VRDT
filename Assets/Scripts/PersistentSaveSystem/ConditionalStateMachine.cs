using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PersistentSaveSystem
{

    public class ConditionalStateMachine : MonoBehaviour
    {
        //not really a state machine, since any state goes into any other one but whatever
        //could not think of a better name + we have no other state machines in the proj anyway

        [System.Serializable]
        public class ConditionalEvent
        {
            public string Name;
            public UnityEvent Event;
            public string[] Condition;
            public void Invoke()
            {
                foreach (string _condition in Condition)
                {
                    if (PlayerPrefs.GetInt(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + _condition, 0) == 0)
                    {
                        Debug.Log("Skipping " + Name);
                        return;
                    }
                }
                Debug.Log(Name + " is getting completed!");
                Event.Invoke();
            }
        }


        [System.Serializable]
        public class State
        {
            public string Name;
            public ConditionalEvent[] Events;
            public ConditionalEvent[] OnPicked;
            public ConditionalEvent[] OnLeft;

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
                Debug.Log(Name + " just got picked!");
                foreach (ConditionalEvent _ce in OnPicked)
                {
                    _ce.Invoke();
                }
            }

            public void Left()
            {
                foreach (ConditionalEvent _ce in OnLeft)
                {
                    _ce.Invoke();
                }

            }
        }

        [SerializeField] State[] States;
        private int CurrentState;

        private void Start()
        {
            States[CurrentState].Picked();
            SwitchState(PlayerPrefs.GetInt(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "SelectedStage", 0));
        }

        public void SwitchState(int _State)
        {
            if (States.Length > _State)
            {
                PlayerPrefs.SetInt(PlayerPrefs.GetInt("CurrentPlayerID", 0).ToString() + "SelectedStage", _State);
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

}
