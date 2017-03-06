using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCPickupState : IPlayableCharacterState
{
    private readonly StatePatternPlayableCharacter player;
    public PCPickupState(StatePatternPlayableCharacter pcStateController)
    {
        player = pcStateController;
    }

    public void OnEnterState()
    {

    }

    public void OnUpdateState()
    {
        player.PCAnimator.SetBool("isPickingUp", false);
        
        //Check for Movement
        if (player.MovementVector != Vector3.zero)
        {
            if (player.IsBlocking)
            {
                player.PCAnimator.SetBool("isBlocking", true);
                OnExitState(player.blockMoveState);
            }
            else
            {
                OnExitState(player.moveState);
            }

        }
        else
        {
            if (player.IsBlocking)
            {
                player.PCAnimator.SetBool("isBlocking", true);
                OnExitState(player.blockIdleState);
            }
            else
            {
                player.PCAnimator.SetInteger("Movement", 0);
                OnExitState(player.idleState);
            }
        }
        
    }

    public void OnExitState(IPlayableCharacterState newState)
    {
        player.CurrentState = newState;
    }
}
