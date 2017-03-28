using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCIdleState : IPlayableCharacterState
{
    private readonly StatePatternPlayableCharacter player;
    public PCIdleState(StatePatternPlayableCharacter pcStateController)
    {
        player = pcStateController;
    }

    public void OnEnterState()
    {
        player.NavAgent.velocity = Vector3.zero;
        player.NavAgent.ResetPath();
        player.NavAgent.Stop();

        player.PCAnimator.applyRootMotion = true;        
        player.PCAnimator.SetBool("IsMoving", false);
        //player.PCAnimator.SetBool("isBlocking", player.IsBlocking);
    }

    public void OnUpdateState()
    {  
        if(player.HasTargetPosition)
        {
            OnExitState(player.moveState);
        }
        else
        {
            if(player.HasPickupTarget)
            {
                OnExitState(player.pickUpState);
            }
        }

        

        ////Check for Movement
        //if (player.MovementVector != Vector3.zero)
        //{
        //    OnExitState(player.moveState);
        //}
        //else
        //{
        //    player.PCAnimator.SetBool("isBlocking", player.IsBlocking);
        //}
    }

    public void OnExitState(IPlayableCharacterState newState)
    {
        player.CurrentState = newState;
    }
}
