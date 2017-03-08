using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCMoveState : IPlayableCharacterState
{
    private readonly StatePatternPlayableCharacter player;
    public PCMoveState(StatePatternPlayableCharacter pcStateController)
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
            OnExitState(player.attackState);
        }

        //Check if We're Picking Up Items
        if (player.IsPickingUp)
        {
            player.PCAnimator.SetBool("isPickingUp", true);
            OnExitState(player.pickUpState);
        }       
        if(player.IsRolling)
        {
            if (PCAttributes.Instance.CheckIfPCHasEnoughStamina(player.RollAction.ActionCost))
            {
                player.PCAnimator.SetBool("isRolling", true);
                OnExitState(player.rollState);
            }
        }
        else 
        {
            //Check we're not standing still
            if(player.MovementVector != Vector3.zero)
            {
                player.transform.rotation = Quaternion.LookRotation(player.MovementVector);

                //Check if We're Blocking
                if (player.IsBlocking)
                {
                    player.PCAnimator.SetBool("isBlocking", true);
                    OnExitState(player.blockMoveState);
                }

                player.PCAnimator.SetInteger("Movement", 1);

            }
            else
            {
                //Check if We're Blocking
                if(player.IsBlocking)
                {
                    player.PCAnimator.SetBool("isBlocking", true);
                    OnExitState(player.blockIdleState);
                }
                else
                {
                    player.PCAnimator.SetBool("isBlocking", false);
                    OnExitState(player.idleState);
                }

                player.PCAnimator.SetInteger("Movement", 0);
                
            }           
        }
    }

    public void OnExitState(IPlayableCharacterState newState)
    {
        player.CurrentState = newState;
    }
}
