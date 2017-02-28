using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCWalkState : IPlayableCharacterState
{
    private readonly StatePatternPlayableCharacter player;
    public PCWalkState(StatePatternPlayableCharacter pcStateController)
    {
        player = pcStateController;
    }

    public void OnEnterState()
    {

    }

    public void OnUpdateState()
    {
        //Check for Rolling Input
        if(player.IsRolling)
        {
            //Check We Have Enough Stamina
            if(PCAttributes.Instance.CheckIfPCHasEnoughStamina(player.SlowRollAction.ActionCost))
            {
                player.PCAnimator.SetBool("isRolling", true);
                OnExitState(player.slowRollState);
            }
        }
        else
        {
            //Check for Movement
            if (player.MovementVector != Vector3.zero)
            {
                //Check for Sprinting Input
                if (player.IsSprinting)
                {
                    //Check if we Have Enough Stamina
                    if (PCAttributes.Instance.CheckIfPCHasEnoughStamina(player.SprintAction.ActionCost))
                    {
                        OnExitState(player.sprintState);
                    }
                }

                player.PCAnimator.SetInteger("Movement", 1);

                //Check that we're not currently rolling
                AnimatorStateInfo currentState = player.PCAnimator.GetCurrentAnimatorStateInfo(0);

                player.transform.rotation = Quaternion.LookRotation(player.MovementVector);
            }
            else //Transition to Idle State
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
