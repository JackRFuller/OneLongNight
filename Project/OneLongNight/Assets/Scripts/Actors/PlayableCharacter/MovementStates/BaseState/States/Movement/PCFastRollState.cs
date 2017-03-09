using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCFastRollState : IPlayableCharacterState
{
    private readonly StatePatternPlayableCharacter player;
	public PCFastRollState(StatePatternPlayableCharacter pcStateController)
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
            player.PCAnimator.SetInteger("Movement", 1);

            //Check if We're Still Sprinting
            if (player.IsSprinting)
            {
                //Check we've got enough stamina
                if(PCAttributes.Instance.CheckIfPCHasEnoughStamina(player.SprintAction.ActionCost))
                {
                    player.PCAnimator.SetBool("isSprinting", true);                    
                    OnExitState(player.sprintState);
                }
                else //Walk
                {
                    player.PCAnimator.SetBool("isSprinting", false);
                    
                    OnExitState(player.walkState);
                }
               
            }
            else
            {
                player.PCAnimator.SetBool("isSprinting", false);               
                OnExitState(player.walkState);

            }
            
        }
        else //Go To Idle
        {
            player.PCAnimator.SetInteger("Movement", 0);
            OnExitState(player.idleState);
        }
    }

    public void OnExitState(IPlayableCharacterState newState)
    {
        player.PCAnimator.SetBool("isRolling", false);
        player.CurrentState = newState;
    }
}
