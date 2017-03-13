using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCPickupState : IPlayableCharacterState
{
    private readonly StatePatternPlayableCharacter player;
    public PCPickupState(StatePatternPlayableCharacter pcStateController)
    {
        player = pcStateController;
    }

    private const float timerStartTime = 0.6f;
    private float timer;

    public void OnEnterState()
    {
        timer = timerStartTime;

        Vector3 lookPos = PCItemInventoryHandler.foundItem.transform.position - player.transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);

        player.transform.rotation = rotation;
    }

    public void OnUpdateState()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            //Check for Movement
            if (player.MovementVector != Vector3.zero)
            {
                player.PCAnimator.SetInteger("Movement", 1);

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
        player.PCAnimator.SetBool("isPickingUp", false);
        player.CurrentState = newState;
    }
}
