using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCSlowRollState : IPlayableCharacterState
{

    private readonly StatePatternPlayableCharacter player;
    public PCSlowRollState(StatePatternPlayableCharacter pcStateController)
    {
        player = pcStateController;
    }

    public void OnEnterState()
    {

    }

    public void OnUpdateState()
    {
        //Check for Movement
        if (player.MovementVector != Vector3.zero)
        {
            player.PCAnimator.SetInteger("Movement", 1);
            OnExitState(player.walkState);
        }
        else
        {
            player.PCAnimator.SetInteger("Movement", 0);
            OnExitState(player.idleState);
        }

    }

    public void OnExitState(IPlayableCharacterState newState)
    {
        player.PCAnimator.SetBool("isRolling", false);
        player.CurrentState = newState;

    }



}
