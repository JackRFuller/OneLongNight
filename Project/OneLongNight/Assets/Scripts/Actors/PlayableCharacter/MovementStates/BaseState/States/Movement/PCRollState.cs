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
        //Set Rotation
        player.transform.rotation = Quaternion.LookRotation(player.MovementVector);

        player.PCAnimator.SetBool("isRolling", true);
        PCAttributes.Instance.RemoveStamina(player.RollAction.ActionCost);
        timer = timerStartTime;
    }

    public void OnUpdateState()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            //Check if we have movement
            if (player.MovementVector != Vector3.zero)
            {
                 OnExitState(player.moveState);
            }
            else
            {
                OnExitState(player.idleState);
            }
        }
    }

    public void OnExitState(IPlayableCharacterState newState)
    {
        player.PCAnimator.SetBool("isRolling", false);
        player.CurrentState = newState;
    }


}
