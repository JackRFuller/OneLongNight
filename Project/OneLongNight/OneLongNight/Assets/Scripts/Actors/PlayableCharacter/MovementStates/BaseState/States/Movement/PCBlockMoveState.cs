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
        //Check if We're Rolling
        if(player.IsRolling)
        {
            if(PCAttributes.Instance.CheckIfPCHasEnoughStamina(player.RollAction.ActionCost))
            {
                player.PCAnimator.SetBool("isRolling", true);
                OnExitState(player.rollState);
            }
        }
        else
        {
            //Check for Movement
            if(player.MovementVector != Vector3.zero)
            {
                //Check that we're not currently rolling
                AnimatorStateInfo currentState = player.PCAnimator.GetCurrentAnimatorStateInfo(0);

                if (currentState.fullPathHash == Animator.StringToHash("Base Layer.BlockMove"))
                    player.transform.rotation = Quaternion.LookRotation(player.MovementVector);

                //Check if we're not blocking
                if (!player.IsBlocking)
                {
                    player.PCAnimator.SetBool("isBlocking", false);
                    OnExitState(player.moveState);
                }

                player.PCAnimator.SetInteger("Movement", 1);


                
            }
        }
    }

    public void OnExitState(IPlayableCharacterState newState)
    {
        player.CurrentState = newState;
    }
}
