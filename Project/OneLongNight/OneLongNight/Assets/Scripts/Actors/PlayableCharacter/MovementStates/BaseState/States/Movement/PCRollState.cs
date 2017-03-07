using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCRollState : IPlayableCharacterState
{
    private readonly StatePatternPlayableCharacter player;
    public PCRollState(StatePatternPlayableCharacter pcStateController)
    {
        player = pcStateController;
    }

    private const float timerStartTime = 1.1f;
    private float timer;

    public void OnEnterState()
    {
        PCAttributes.Instance.RemoveStamina(player.RollAction.ActionCost);
        timer = timerStartTime;
    }

    public void OnUpdateState()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
            Debug.Log("Wait For Roll To Finish");
        }
        else
        {
            //Check if we have movement
            if (player.MovementVector != Vector3.zero)
            {
                player.PCAnimator.SetInteger("Movement", 1);

                //Check if we're blocking 
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
                player.PCAnimator.SetInteger("Movement", 0);

                //Check if We're Blocking
                if (player.IsBlocking)
                {
                    player.PCAnimator.SetBool("isBlocking", true);
                    OnExitState(player.blockIdleState);
                }
                else
                {
                    player.PCAnimator.SetBool("isBlocking", false);
                    OnExitState(player.idleState);
                }
            }
        }
    }

    public void OnExitState(IPlayableCharacterState newState)
    {
        player.PCAnimator.SetBool("isRolling", false);
        player.CurrentState = newState;
    }


}
