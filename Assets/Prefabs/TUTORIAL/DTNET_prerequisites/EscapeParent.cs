using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeParent : MonoBehaviour
{
    Transform grandpa;
    // Start is called before the first frame update
    void Start()
    {
            grandpa = transform.parent.parent;

    }

    public void Escape()
    {
        if (!PhotonManager._viewerApp)
        {
            transform.SetParent(grandpa);
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
