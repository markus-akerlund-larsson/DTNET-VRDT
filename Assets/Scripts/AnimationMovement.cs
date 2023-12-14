using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class AnimationMovement : MonoBehaviour
{
    public Animator animator;    

    [SerializeField] private float speed = 1;
    [SerializeField] List<Transform> moveListPonints = new List<Transform>();
    [SerializeField] Transform [] stopPoint;     
    public Action walk;  
    public UnityEvent OnStartMove;
    public UnityEvent OnFinishMove;    
    private Transform moveObj;
    Transform nextPoint;
    int indexPoint;
    bool startMoving;
    bool startEvent;
    public bool onPlace;
    bool rotate;

    int stopPointIndex;
    // Start is called before the first frame update
    void Start()
    {
        moveObj = animator.transform;

        if (GameObject.Find("Wheelchair"))
        {
            Transform wheelchair = GameObject.Find("Wheelchair").transform;
            GameObject point = new GameObject();
            point.transform.parent = wheelchair;
            point.transform.localPosition = new Vector3(0, 0.324f, 1.02f);
            point.transform.localEulerAngles = new Vector3(0, 180, 0);
            moveListPonints.Add(point.transform);
        }
    }

    private void Move()
    {
        nextPoint = moveListPonints[indexPoint];

        Vector3 newDirection = Vector3.RotateTowards(moveObj.forward, nextPoint.position - moveObj.position, speed *3* Time.deltaTime, 1f);
        moveObj.rotation = Quaternion.LookRotation(newDirection);
        moveObj.position = Vector3.MoveTowards(moveObj.position, nextPoint.position, speed * Time.deltaTime);

        if (moveObj.position == nextPoint.position)
        {
            if (indexPoint < moveListPonints.Count - 1)
                indexPoint++;
        }

        if (stopPoint.Length==0)
        {
            if (moveObj.position == moveListPonints[moveListPonints.Count - 1].position)
            {
                moveObj.rotation = moveListPonints[moveListPonints.Count - 1].rotation;
                startMoving = false;
                onPlace = true;
                OnFinishMove.Invoke();
            }
        }
        if (stopPoint.Length>0)
        {
            if (moveObj.position == stopPoint[stopPointIndex].position)
            {
                startMoving = false;
                onPlace = true;
                OnFinishMove.Invoke();
            }
        }

    }

    public void AnimationPutOnPulse() 
    {
        animator.SetTrigger("StandUp");
        FindObjectOfType<SensorController>(true).sensorAnimation = true;
    }
    public void AnimationThermometer()
    {
        animator.SetTrigger("StandUp");
        FindObjectOfType<SensorController>(true).thermometerAnimation= true;
    }
    public void StopEvent() 
    {
        if (moveObj.position == stopPoint[0].position) 
        {
            moveObj.rotation = stopPoint[0].rotation;
            animator.SetTrigger("Take");
            Invoke("ContinueMove", 4.7f);
        }

        if (moveObj.position == stopPoint[1].position) 
        {
            moveObj.rotation = stopPoint[1].rotation;
            animator.SetTrigger("Give");
            Invoke("ContinueMove", 2f);
        }

        if (moveObj.position == stopPoint[2].position) 
        {
            moveObj.rotation = stopPoint[2].rotation;
            animator.SetTrigger("SitDown");
            animator.applyRootMotion = true;
        }

    }
    void ContinueMove() 
    {
        animator.SetTrigger("Walking");
        stopPointIndex++;
        onPlace = false;
        startMoving = true;
    }
    
    public void Rotate() 
    {
        animator.SetTrigger("Turn");
        moveObj.transform.parent = moveListPonints[moveListPonints.Count - 1].parent;
        rotate = true;
        startMoving = false;
        startEvent = false;
    }
    public void StartMove(bool startMoving)
    {
        this.startMoving = startMoving;
        walk?.Invoke();
    }

    public void StartWheelChairANimation() 
    {
        animator.SetTrigger("StandUp");

    }
    // Update is called once per frame
    void Update()
    {

        if (animator.GetCurrentAnimatorStateInfo(0).IsTag("Walk") && !startMoving && !onPlace)
        {
            //if (GetComponent<NavMeshAgent>())
            //{
            //    GetComponent<NavMeshAgent>().destination = MoveListPonints[0].position;
            //}
            animator.applyRootMotion = false;
            startMoving = true;
        }

        //if (GetComponent<NavMeshAgent>().remainingDistance<= GetComponent<NavMeshAgent>().stoppingDistance)
        //{
        //    GetComponent<NavMeshAgent>().enabled = false;
        //    rotate = true;
        //}

        if (rotate)
        {
            if (moveObj.transform.localEulerAngles.y > 0)
            {
                moveObj.Rotate(0, 100 * Time.deltaTime, 0);
                if (moveObj.transform.localEulerAngles.y > 359 || moveObj.transform.localEulerAngles.y<5)
                {
                    moveObj.transform.localEulerAngles = new Vector3(0, 0, 0);
                    animator.SetTrigger("SitDown");
                    animator.applyRootMotion = true;
                    rotate = false;
                }
            }
            if (moveObj.transform.localEulerAngles.y < 0)
            {
                moveObj.Rotate(0, -100 * Time.deltaTime, 0);
            }
        }


        if (startMoving)
        {
            if (!startEvent)
            {
                OnStartMove.Invoke();
                startEvent = true;
            }
            Move();
        }

    }
}
