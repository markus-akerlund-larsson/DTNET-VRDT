using System.Collections;
using System.Collections.Generic;
using RecordedScenario;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HeadCharacterFollow : MonoBehaviour
{ 
    [SerializeField] float interpolationSpeed=1;

    Transform player;
    Transform defaultPos;
    Transform playerPos;
    Transform m_trLookAt = null;
    Transform m_Transform;
    Vector3 m_vecInitPosition;
    Vector3 m_vecInitEuler;    
    Vector3 startPos;
    Quaternion startRot;
    float m_LookAtWeight = 0;
    protected Animator m_Animator;
    public bool inArea;
    public bool alawaysInArea;
    Transform targetPos;
    void Start()
    {
        if (!Animator)
            Animator = GetComponent<Animator>();

        player = Camera.main.transform;

        defaultPos = new GameObject().transform;
        defaultPos.gameObject.name = "HeadDefaultPosition";
        defaultPos.parent = transform;
        defaultPos.localPosition = new Vector3(0, 1.18f, 0.1f);
        playerPos = new GameObject().transform;
        playerPos.gameObject.name = "HeadFollowTarget";


        m_Transform = transform;
        m_vecInitEuler = m_Transform.localEulerAngles;
        m_vecInitPosition = m_Transform.localPosition;
        m_trLookAt = defaultPos;
        inArea = false;

        startPos = defaultPos.position;
        startRot = defaultPos.rotation;

        if (FindAnyObjectByType<RecordedScenarioText>() && SceneManager.GetActiveScene().name != "RecordedShiftChange")
        {
            this.enabled = false;
        }
    }
    private void Update()
    {
        if (defaultPos.position != startPos)
            defaultPos.position = Vector3.MoveTowards(defaultPos.position, startPos, interpolationSpeed * Time.deltaTime);
        if (defaultPos.rotation != startRot)
            defaultPos.rotation = Quaternion.RotateTowards(defaultPos.rotation, startRot, 10*interpolationSpeed * Time.deltaTime);

        if (playerPos.position != player.position)
            playerPos.position = Vector3.MoveTowards(playerPos.position, player.position, interpolationSpeed * Time.deltaTime);
        if (defaultPos.rotation != player.rotation)
            playerPos.rotation = Quaternion.RotateTowards(playerPos.rotation, player.rotation, 10 * interpolationSpeed * Time.deltaTime);
    }
    public void ExitArea() 
    {
        defaultPos.position = playerPos.position;
        defaultPos.rotation = playerPos.rotation;
        inArea = false;
    }
    public void EnterArea()
    {
        playerPos.position = defaultPos.position;
        playerPos.rotation = defaultPos.rotation;
        //m_LookAtWeight = 0;
        inArea = true;
      
    }
    public Animator Animator
    {
        get => m_Animator;
        set => m_Animator = value;
    }
    void OnAnimatorIK(int layerIndex)
    {
        if (inArea && !alawaysInArea) m_trLookAt = playerPos;

        if (!inArea && !alawaysInArea) m_trLookAt = defaultPos;

        if (alawaysInArea) m_trLookAt = targetPos;

        if (!Animator)
            return;
        if (m_trLookAt == null)
        {
            _StopLookAt();
            return;
        }
        _StartLookAt(m_trLookAt.position);
    }
   
    void _StartLookAt(Vector3 lookPos)
    {
        m_LookAtWeight = Mathf.Clamp(m_LookAtWeight + 0.01f, 0, 0.5f);
        Animator.SetLookAtWeight(m_LookAtWeight);
        Animator.SetLookAtPosition(lookPos);
    }
    void _StopLookAt()
    {
        m_Transform.localPosition = m_vecInitPosition;
        m_Transform.localEulerAngles = m_vecInitEuler;
        m_LookAtWeight = Mathf.Clamp(m_LookAtWeight - 0.01f, 0, 1);
        Animator.SetLookAtWeight(m_LookAtWeight);
    }

    public void ChangeHeadFollow(Transform followPos) 
    {
        targetPos=followPos;
        alawaysInArea = true;
    
    }
}
