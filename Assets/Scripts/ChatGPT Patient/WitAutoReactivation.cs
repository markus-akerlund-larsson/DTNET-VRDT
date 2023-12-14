using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Meta.WitAi;
using Meta.WitAi.TTS.Utilities;

public class WitAutoReactivation : MonoBehaviour
{
    WitService _wit;
    public bool temporarilyIgnore;

    public void Ignore(bool ignore)
    {
        temporarilyIgnore = ignore;
    }

    private void Start()
    {
        _wit = GetComponent<WitService>();
        if(_wit != null)
            _wit.Activate();
    }

    void Update()
    {
        bool _someoneIsTalking = false;
        TTSSpeaker[] _allSpeakers = FindObjectsOfType<TTSSpeaker>();
        foreach(TTSSpeaker _s in _allSpeakers)
        {
            if(_s.AudioSource.isPlaying == true)
            {
                _someoneIsTalking = true;
                break;
            }
        }

        if(_wit == null)
            _wit = GetComponent<WitService>();

        if (!_wit.Active)
        {
            if (!_someoneIsTalking && !temporarilyIgnore)
                _wit.ActivateImmediately();
        }
        else
        {
            if (_someoneIsTalking || temporarilyIgnore)
                _wit.DeactivateAndAbortRequest();
        }
    }
}
