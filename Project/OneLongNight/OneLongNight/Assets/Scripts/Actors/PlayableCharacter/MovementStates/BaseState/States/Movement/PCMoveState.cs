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
        //Check if We're Picking Up Items
        if(player.IsPickingUp)
        {
            player.PCAnimator.SetBool("isPickingUp", true);
            OnExitState(player.pickUpState);
        }

        //Check if Player is Rolling
        if(player.IsRolling)
        {
            if(PCAttributes.Instance.CheckIfPCHasEnoughStamina(player.RollAction.ActionCost))
            {
                player.PCAnimator.SetBool("isRolling", true);
                //Add Roll State
                OnExitState(player.rollState);
            }
        }
        else
        {
            //Check we're not standing still
            if(player.MovementVector != Vector3.zero)
            {
                AnimatorStateInfo currentState = player.PCAnimator.GetCurrentAnimatorStateInfo(0);

                if (currentState.fullPathHash == Animator.StringToHash("Base Layer.Move"))
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
