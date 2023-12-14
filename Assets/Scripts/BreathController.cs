using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathController : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float breathingStartValue;
    [SerializeField] float breathingEndValue;
    public bool breathing;
    bool _return;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (breathing)
        {
            if (transform.localScale.y < breathingEndValue && !_return)
            {
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y + (speed * Time.deltaTime), transform.localScale.z);
            }
            if (transform.localScale.y >= breathingEndValue && !_return)
            {
                _return = true;
            }
            if (transform.localScale.y > breathingStartValue && _return)
            {
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y - (speed * Time.deltaTime), transform.localScale.z);
            }
            if (transform.localScale.y <= breathingStartValue && _return)
            {
                _return = false;
            }

            Vector3 childScale = new Vector3(1 / transform.localScale.x, 1 / transform.localScale.y, 1 / transform.localScale.z);

            foreach (Transform transform in GetComponentInChildren<Transform>())
            {
                transform.localScale = childScale;
            }
        }
    }
}
