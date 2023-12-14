using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScenarioTaskSystem
{
    public abstract class ResultHandler : MonoBehaviour
    {
        public abstract void HandleResult(Scenario _scenario);
    }

}
