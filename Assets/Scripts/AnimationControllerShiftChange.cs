using System;
using System.Collections;
using System.Collections.Generic;
using RecordedScenario;
using UnityEngine;

public class AnimationControllerShiftChange : MonoBehaviour
{
    [SerializeField] RuntimeAnimatorController runtimeAnimatorController;
    [SerializeField] Animator offgoingNurse;
    [SerializeField] Animator oncomingNurse;
    //[SerializeField] List<Transform> movePonintsOffgoingNurse = new List<Transform>();
    //[SerializeField] List<Transform> movePonintsOncomingNurse = new List<Transform>();
    //[SerializeField] Transform[] stopPoint;
    [SerializeField] float speedMove=1;
    [SerializeField] float speedRotation = 300;
    [SerializeField] Transform monitor;
    [SerializeField] Transform oxygen;
    [SerializeField] Transform monitor2;
    [SerializeField] Transform drip;
    [SerializeField] Transform leg;
    [SerializeField] Transform ambu;
    [SerializeField] Transform record;

    Animator nurse;
    Transform point;
    bool isMoving;
    bool isRotating;
    Action stopWalk;
    string triggerAnimation;

    private void Start()
    {
        offgoingNurse.runtimeAnimatorController = runtimeAnimatorController;
        offgoingNurse.applyRootMotion = false;
        offgoingNurse.GetComponent<HeadCharacterFollow>().ChangeHeadFollow(oncomingNurse.GetBoneTransform(HumanBodyBones.Head));
        oncomingNurse.runtimeAnimatorController = runtimeAnimatorController;
        oncomingNurse.applyRootMotion = false;
        oncomingNurse.GetComponent<HeadCharacterFollow>().ChangeHeadFollow(offgoingNurse.GetBoneTransform(HumanBodyBones.Head));

    }
    public void SwitchAnimation(string key) 
    {
        if (key.Contains("on"))
            nurse = oncomingNurse;
        if (key.Contains("off"))
            nurse = offgoingNurse;

        switch (key)
        {
            case "off_move_to_monitor":
                nurse.GetComponent<HeadCharacterFollow>().ChangeHeadFollow(monitor.GetChild(0));
                nurse.SetTrigger("Walk");
                point = monitor;
                isMoving = true;
                isRotating = true;
                break;

            case "off_point_to_oxygen":
                nurse.GetComponent<HeadCharacterFollow>().ChangeHeadFollow(oxygen.GetChild(0));
                nurse.SetTrigger("Walk");
                triggerAnimation = "Point";
                stopWalk += SetTriggerAnimation;
                point = oxygen;
                isMoving = true;
                isRotating = true;
                break;
            case "on_ point_to_monitoring":
                nurse.GetComponent<HeadCharacterFollow>().ChangeHeadFollow(monitor2);
                nurse.SetTrigger("Point");
                point = monitor2;
                isRotating = true;
                break;
            case "off_look_at_monitoring":
                nurse.GetComponent<HeadCharacterFollow>().ChangeHeadFollow(monitor2);
                point = monitor2;
                //isRotating = true;
                break;
            case "on_point_to_drop":
                nurse.GetComponent<HeadCharacterFollow>().ChangeHeadFollow(drip);
                point = drip;
                isRotating = true;
                break;
            case "on_check_drip":
                nurse.GetComponent<HeadCharacterFollow>().ChangeHeadFollow(drip);
                nurse.SetTrigger("Walk");
                point = drip;
                isRotating = true;
                isMoving = true;
                break;
            case "off_point_to_leg":
                nurse.GetComponent<HeadCharacterFollow>().ChangeHeadFollow(leg);
                nurse.SetTrigger("Point");
                point = leg;
                isRotating = true;
                break;
            case "on_look_at_leg":
                nurse.GetComponent<HeadCharacterFollow>().ChangeHeadFollow(leg);
                point = leg;
                isRotating = true;
                break;
            case "off_show_ambu":
                nurse.GetComponent<HeadCharacterFollow>().ChangeHeadFollow(ambu);
                nurse.SetTrigger("Show");
                point = ambu;
                isRotating = true;
                break;
            case "off_point_record":
                nurse.GetComponent<HeadCharacterFollow>().ChangeHeadFollow(record.GetChild(0));
                nurse.SetTrigger("Walk");
                triggerAnimation = "Point";
                stopWalk += SetTriggerAnimation;
                point = record;
                isMoving = true;
                isRotating = true;
                break;

            default:
                break;
        }
    }
    void SetTriggerAnimation() 
    {
        nurse.SetTrigger(triggerAnimation);
        stopWalk -= SetTriggerAnimation;
    }

    void Rotate() 
    {
        nurse.transform.rotation = Quaternion.RotateTowards(nurse.transform.rotation, point.rotation, speedRotation * Time.deltaTime);
        if (nurse.transform.rotation == point.rotation)
            isRotating = false;

        //Vector3 newRotateDirection = Vector3.RotateTowards(nurse.transform.forward, point.position - nurse.transform.position, speedMove * 3 * Time.deltaTime, 1f);
        //nurse.transform.rotation = Quaternion.LookRotation(newRotateDirection);
        //nurse.transform.localEulerAngles = new Vector3(nurse.transform.localEulerAngles.x, nurse.transform.localEulerAngles.y, nurse.transform.localEulerAngles.z);
    }
    private void Move()
    {
        Vector3 newMoveDirection = new Vector3(point.position.x, nurse.transform.position.y, point.position.z);
        nurse.transform.position = Vector3.MoveTowards(nurse.transform.position, newMoveDirection, speedMove * Time.deltaTime);
        if (nurse.transform.position == newMoveDirection)
        {
            nurse.GetComponent<Animator>().SetTrigger("Idle");
            isMoving = false;
            stopWalk?.Invoke();
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
            Move();
        if (isRotating)
            Rotate();
    }
}
