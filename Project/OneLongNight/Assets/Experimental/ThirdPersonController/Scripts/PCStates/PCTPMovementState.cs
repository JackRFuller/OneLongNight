using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCTPMovementState : IPlayableCharacterState
{
    private readonly PCStateManager player;
    public PCTPMovementState(PCStateManager pcStateController)
    {
        player = pcStateController;
    }

    public void OnEnterState()
    {
        
        player.PlayerAnim.SetBool("WalkingForward", true);
    }

    public void OnUpdateState()
    {
        if(player.Vertical == 0)
        {
            OnExitState(player.idleState);
        }

        if (player.Horizontal >= .1 || player.Horizontal <= -.1f)
        {
            player.transform.Rotate(0, (player.Horizontal * 3),0);
        }
    }

    public void OnExitState(IPlayableCharacterState newState)
    {
        player.PlayerAnim.SetBool("WalkingForward", false);
        player.currentState = newState;
    }
}
