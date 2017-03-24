using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCIdleState : IPlayableCharacterState
{
    private readonly StatePatternPlayableCharacter player;
    public PCIdleState(StatePatternPlayableCharacter pcStateController)
    {
        player = pcStateController;
    }

    public void OnEnterState()
    {
        player.NavAgent.Stop();
        player.Anim.SetBool("IsMoving", false);
    }

    public void OnUpdateState()
    {
        //Check if we have target position
        if(player.HasTargetMovePosition)
        {
            //Go To Move State
            OnExitState(player.moveState);
        }
        
    }

    public void OnExitState(IPlayableCharacterState newState)
    {
        player.CurrentState = newState;
    }
}
