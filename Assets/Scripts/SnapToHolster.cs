using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SnapToHolster : MonoBehaviour
{
    [SerializeField] Transform followTransform;
    [SerializeField] float speed;
    [SerializeField] UnityEvent OnRelease;
    [SerializeField] UnityEvent OnCameBack;

    private Transform prevParent;
    private Quaternion initRotation;
    private float initMagnitude;

    private Rigidbody rb;

    private bool isFollowing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        prevParent = transform.parent;
    }

    void Update()
    {
        if (prevParent != followTransform && transform.parent == followTransform)
        {
            initRotation = transform.rotation;
            initMagnitude = (followTransform.position - transform.position).magnitude;
            OnRelease.Invoke();
            isFollowing = true;
            rb.isKinematic = true;
        }
        if (isFollowing)
        {
            Vector3 direction = followTransform.position - transform.position;
            transform.position += direction.normalized * Mathf.Min(direction.magnitude, Time.deltaTime * speed);

            transform.rotation = Quaternion.Lerp(initRotation, followTransform.rotation, 1 - (direction.magnitude / initMagnitude));


            if (direction.magnitude < 0.000001f)
            {
                transform.parent = followTransform;
                isFollowing = false;
                OnCameBack.Invoke();
            }
        }
        else
        {
            if(transform.parent == followTransform)
            {
                transform.localPosition = Vector3.zero;
                transform.rotation = followTransform.rotation;
            }
        }
        prevParent = transform.parent;
    }

}
