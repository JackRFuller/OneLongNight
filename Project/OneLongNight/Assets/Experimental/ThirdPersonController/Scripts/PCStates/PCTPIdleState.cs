using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCTPIdleState : IPlayableCharacterState
{

    private readonly PCStateManager player;
    public PCTPIdleState(PCStateManager pcStateController)
    {
        player = pcStateController;
    }

    public void OnEnterState()
    {
        player.PlayerAnim.SetBool("IsIdle", true);
    }

    public void OnUpdateState()
    {
        if(player.Vertical != 0)
        {
            OnExitState(player.moveState);
        }
    }

    public void OnExitState(IPlayableCharacterState newState)
    {
        player.PlayerAnim.SetBool("IsIdle", false);
        player.currentState = newState;
    }
}
