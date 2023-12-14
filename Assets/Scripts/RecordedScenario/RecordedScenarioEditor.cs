#if UNITY_EDITOR 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RecordedScenario
{
    [CustomEditor(typeof(RecordedScenarioText))]
    public class RecordedScenarioEditor : Editor
    {
        override public void OnInspectorGUI()
        {
            RecordedScenarioText _scenario = (RecordedScenarioText)target;
            if (GUILayout.Button("Load Scenario"))
            {
                _scenario.InitiateSetup();
            }
            if (GUILayout.Button("Stop Loading"))
            {
                _scenario.BreakSetup();
            }
            DrawDefaultInspector();
        }
    }

    
}
#endif
