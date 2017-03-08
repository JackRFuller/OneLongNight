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
        if(player.IsAttacking)
        {
            //Check if We Have Enough Stamina to Attack
            OnExitState(player.attackState);
        }

        //Check For Roll
        if(player.IsRolling)
        {
            //Check We Have Enough Stamina
            if(PCAttributes.Instance.CheckIfPCHasEnoughStamina(player.RollAction.ActionCost))
            {
                player.PCAnimator.SetBool("isRolling", true);
                OnExitState(player.rollState);
            }
        }

        //Check if Player is Picking Up An Item
        if(player.IsPickingUp)
        {
            player.PCAnimator.SetBool("isPickingUp", true);
            OnExitState(player.pickUpState);
        }

        //Check for Movement
        if (player.MovementVector != Vector3.zero)
        {
            //player.PCAnimator.SetInteger("Movement", 1);

            if(player.IsBlocking)
            {
                player.PCAnimator.SetBool("isBlocking", true);
                OnExitState(player.blockMoveState);
            }
            else
            {
                player.PCAnimator.SetBool("isBlocking", false);
                OnExitState(player.moveState);
            }
            
        }
        else
        {
            if(player.IsBlocking)
            {
                player.PCAnimator.SetBool("isBlocking", true);
                OnExitState(player.blockIdleState);
            }
        }
    }

    public void OnExitState(IPlayableCharacterState newState)
    {
        player.CurrentState = newState;
    }
}
