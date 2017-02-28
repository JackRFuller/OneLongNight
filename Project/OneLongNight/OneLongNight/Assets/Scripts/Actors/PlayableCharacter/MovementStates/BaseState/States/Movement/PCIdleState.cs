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

    }

    public void OnUpdateState()
    {
        //Check For Roll
        if(player.IsRolling)
        {
            //Check We Have Enough Stamina
            if(PCAttributes.Instance.CheckIfPCHasEnoughStamina(player.SlowRollAction.ActionCost))
            {
                player.PCAnimator.SetBool("isRolling", true);
                OnExitState(player.slowRollState);
            }
        }

        //Check for Attack
        if(player.IsTryingToAttack)
        {
			OnExitState(player.standardAttackState);
        }

        //Check for Movement
        if(player.MovementVector != Vector3.zero)
        {
            //Check for Sprinting
            if(player.IsSprinting)
            {
                //Check if Player has Enough Stamina
                if(PCAttributes.Instance.CheckIfPCHasEnoughStamina(player.SprintAction.ActionCost))
                {
                    //Move to Sprint State
                    OnExitState(player.sprintState);

                }
                else //Move to Walking State
                {
                    OnExitState(player.walkState);
                }

            }
            else //Move to Walking State
            {
                OnExitState(player.walkState);
            }
                        
        }
    }

    public void OnExitState(IPlayableCharacterState newState)
    {
        player.CurrentState = newState;
    }
}
