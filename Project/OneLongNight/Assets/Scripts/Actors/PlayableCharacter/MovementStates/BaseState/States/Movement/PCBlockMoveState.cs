using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCBlockMoveState : IPlayableCharacterState
{
    private readonly StatePatternPlayableCharacter player;
    public PCBlockMoveState(StatePatternPlayableCharacter pcStateController)
    {
        player = pcStateController;
    }

    public void OnEnterState()
    {

    }

    public void OnUpdateState()
    {

        if (player.IsAttacking)
        {
            //Check We Have Enough Stamina
            if (PCAttributes.Instance.CheckIfPCHasEnoughStamina(PCItemInventoryHandler.CurrentWeapon.weaponAttackCosts[0]))
            {
                //Check if We Have Enough Stamina to Attack
                OnExitState(player.attackState);
            }
        }

        //Check if We're Picking Up Items
        if (player.IsPickingUp)
        {
            player.PCAnimator.SetBool("isPickingUp", true);
            OnExitState(player.pickUpState);
        }

        //Check if We're Rolling
        if (player.IsRolling)
        {
            if (PCAttributes.Instance.CheckIfPCHasEnoughStamina(player.RollAction.ActionCost))
            {
                player.PCAnimator.SetBool("isRolling", true);
                OnExitState(player.rollState);
            }
        }
        else
        {
            //Check for Movement
            if (player.MovementVector != Vector3.zero)
            {
                player.transform.rotation = Quaternion.LookRotation(player.MovementVector);

                //Check if we're not blocking
                if (!player.IsBlocking)
                {
                    player.PCAnimator.SetBool("isBlocking", false);
                    OnExitState(player.moveState);
                }

                player.PCAnimator.SetInteger("Movement", 1);
            }
            else
            {
                if (!player.IsBlocking)
                {
                    player.PCAnimator.SetBool("isBlocking", false);
                    OnExitState(player.idleState);
                }
                else
                {
                    player.PCAnimator.SetBool("isBlocking", true);
                    OnExitState(player.blockIdleState);
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
