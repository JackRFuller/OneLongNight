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

    private void Start()
    {
        //Get Components
        navAgent = this.GetComponent<NavMeshAgent>();
        anim = this.GetComponent<Animator>();

        //Set Up New States
        idleState = new PCIdleState(this);


        //Set Starting State
        currentState = idleState;
    }

    public override void UpdateNormal()
    {
        if(currentState != lastState)
        {
            currentState.OnEnterState();
            lastState = currentState;
        }
        else
        {
            currentState.OnUpdateState();
        }
    }


}
