using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Rewired;

public class StatePatternPlayableCharacter : BaseMonoBehaviour
{
    //Components
    private NavMeshAgent navAgent;
    public NavMeshAgent NavAgent
    {
        get
        {
            return navAgent;
        }
    }
    private Animator anim;
    public Animator Anim
    {
        get
        {
            return anim;
        }
    }

    //Scripts
    private PCLookAt lookAt;
    public PCLookAt LookAt
    {
        get
        {
            return lookAt;
        }
    }

    //States
    private IPlayableCharacterState currentState;
    public IPlayableCharacterState CurrentState
    {
        get
        {
            return currentState;
        }
        set
        {
            currentState = value;
        }
    }
    private IPlayableCharacterState lastState;

    public PCIdleState idleState;
    public PCMoveState moveState;
    public PCRollState rollState;

    //Movement
    private Vector3 targetMovePosition;
    public Vector3 TargetMovePosition
    {
        get
        {
            return targetMovePosition;
        }
    }
    private bool hasTargetMovePosition;
    public bool HasTargetMovePosition
    {
        get
        {
            return hasTargetMovePosition;
        }
        set
        {
            hasTargetMovePosition = value;
        }
    }

    private void Start()
    {
        //Get Components
        navAgent = this.GetComponent<NavMeshAgent>();
        anim = this.GetComponent<Animator>();

        //Get Scripts
        lookAt = this.GetComponent<PCLookAt>();

        //Set Up New States
        idleState = new PCIdleState(this);
        moveState = new PCMoveState(this);
        rollState = new PCRollState(this);


        //Set Starting State
        currentState = idleState;
    }

    public override void UpdateNormal()
    {
        UpdateStates();

        GetInput();
    }

    private void UpdateStates()
    {
        if (currentState != lastState)
        {
            currentState.OnEnterState();
            lastState = currentState;
        }
        else
        {
            currentState.OnUpdateState();
        }
    }

    private void GetInput()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if(Input.GetMouseButton(0))
            {
                if(hit.collider.tag.Equals("Ground"))
                {
                    targetMovePosition = new Vector3(hit.point.x,
                                                     transform.position.y,
                                                     hit.point.z);
                    hasTargetMovePosition = true;
                }
            }
            if(Input.GetMouseButton(1))
            {
                if (hit.collider.tag.Equals("Ground"))
                {
                    if(CurrentState != rollState)
                    {
                        targetMovePosition = new Vector3(hit.point.x,
                                                     transform.position.y,
                                                     hit.point.z);
                        currentState = rollState;
                    }
                }
            }
        }
        
    }
}
