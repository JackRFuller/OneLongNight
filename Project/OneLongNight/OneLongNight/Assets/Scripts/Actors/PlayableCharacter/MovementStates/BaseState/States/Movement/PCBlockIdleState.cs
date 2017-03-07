using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCBlockIdleState : IPlayableCharacterState
{
    private readonly StatePatternPlayableCharacter player;
    public PCBlockIdleState(StatePatternPlayableCharacter pcStateController)
    {
        player = pcStateController;
    }

    public void OnEnterState()
    {
        Debug.Log("Entered Blocking Idle");
    }

    public void OnUpdateState()
    {
        

        //Check if We're Picking Up Items
        if (player.IsPickingUp)
        {
            player.PCAnimator.SetBool("isPickingUp", true);
            OnExitState(player.pickUpState);
        }

        //Check For Roll
        if (player.IsRolling)
        {
            //Check We Have Enough Stamina
            if (PCAttributes.Instance.CheckIfPCHasEnoughStamina(player.RollAction.ActionCost))
            {
                player.PCAnimator.SetBool("isBlocking", false);
                player.PCAnimator.SetBool("isRolling", true);
                OnExitState(player.rollState);
            }
        }
        else
        {
            //Check for Movement
            if (player.MovementVector != Vector3.zero)
            {
                //CHeck if We're Blocking
                if (player.IsBlocking)
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
                if (!player.IsBlocking)
                {
                    player.PCAnimator.SetBool("isBlocking", false);
                    OnExitState(player.idleState);
                }
                else
                {
                    OnExitState(player.blockIdleState);
                }
            }
        }
    }

    public void OnExitState(IPlayableCharacterState newState)
    {
        
        player.CurrentState = newState;
    }

}
