using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCRollState : IPlayableCharacterState
{
    private readonly StatePatternPlayableCharacter player;
    public PCRollState(StatePatternPlayableCharacter pcStateController)
    {
        player = pcStateController;
    }

    public void OnEnterState()
    {

    }

    public void OnUpdateState()
    {
        //Check if we have movement
        if(player.MovementVector != Vector3.zero)
        {
            //Check if we're blocking 
            player.PCAnimator.SetInteger("Movement", 1);
            OnExitState(player.moveState);
            

        }
        else
        {
            //Go to Idle
        }
    }

    public void OnExitState(IPlayableCharacterState newState)
    {
        player.PCAnimator.SetBool("isRolling", false);
        player.CurrentState = newState;
    }


}
