using System.Collections;
using System.Collections.Generic;
using Autohand;
using UnityEngine;

public class IgnoreAllColliders : MonoBehaviour
{

    public List<Collider> Exceptions;

    // Start is called before the first frame update
    void Start()
    {
        if (Exceptions == null)
        {
            Exceptions = new List<Collider>();
        }
        var hands = FindObjectsOfType<Hand>(true);
        for (int i = 0; i < hands.Length; i++)
        {
            foreach (var col in hands[i].GetComponentsInChildren<Collider>(true))
                Exceptions.Add(col);
        }
        IgnoreAll();
    }

    private void IgnoreAll()
    {
        Collider thisCol = GetComponent<Collider>();
        Collider[] CollidersToIgnore = FindObjectsOfType<Collider>();
        if (CollidersToIgnore != null)
        {
            foreach (Collider col in CollidersToIgnore)
            {
                if (col && col.enabled && !Exceptions.Contains(col))
                {
                    Physics.IgnoreCollision(thisCol, col, true);
                }
            }
        }
    }

    public void UpdateIgnores()
    {
        IgnoreAll();
    }
}
