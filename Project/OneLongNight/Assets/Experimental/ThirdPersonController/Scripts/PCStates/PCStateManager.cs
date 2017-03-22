using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCStateManager : BaseMonoBehaviour
{
    //Components
    private Animator playerAnim;
    public Animator PlayerAnim
    {
        get
        {
            return playerAnim;
        }
    }

    //States
    public PCTPIdleState idleState;
    public PCTPMovementState moveState;
    public PCTPRollState rollState;

    private IPlayableCharacterState lastState;
    public IPlayableCharacterState currentState;

    //Inputs
    public float Horizontal;
    public float Vertical;

    private void Start()
    {
        //Get Components
        playerAnim = this.GetComponent<Animator>();

        idleState = new PCTPIdleState(this);
        moveState = new PCTPMovementState(this);
        rollState = new PCTPRollState(this);        

        currentState = idleState;
    }

    public override void UpdateNormal()
    {
        GetInputs();

        if(currentState != lastState)
        {
            currentState.OnEnterState();
            lastState = currentState;
        }

        currentState.OnUpdateState();
    }

    private void GetInputs()
    {
        Horizontal = Input.GetAxis("Horizontal");
        Vertical = Input.GetAxis("Vertical");

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(currentState != rollState)
            {
                currentState = rollState;
            }
           
        }


    }
}
