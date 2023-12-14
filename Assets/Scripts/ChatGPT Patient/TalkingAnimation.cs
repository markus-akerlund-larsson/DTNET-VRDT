using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkingAnimation : MonoBehaviour
{
    public SkinnedMeshRenderer skinnedMeshRenderer;
    int frequencyBand = 1;    
    AudioPeer audioPeer;
    float multiplier = 2000;


    // Start is called before the first frame update
    void Start()
    {
        audioPeer=GetComponent<AudioPeer>();
    
    }

    // Update is called once per frame
    void Update()
    {
        if (audioPeer == null) return;

        if (skinnedMeshRenderer.transform.parent.name == "YoungFemale")
        {
            skinnedMeshRenderer.SetBlendShapeWeight(7, audioPeer.GetFrequencyBand(frequencyBand) * multiplier / 1.5f);
        }
        if (skinnedMeshRenderer.transform.parent.name == "SeniorFemale")
        {
            skinnedMeshRenderer.SetBlendShapeWeight(8, audioPeer.GetFrequencyBand(frequencyBand) * multiplier);
        }
        else
        {
            skinnedMeshRenderer.SetBlendShapeWeight(67, audioPeer.GetFrequencyBand(frequencyBand) * (multiplier/1.75f));
        }

    }
}
