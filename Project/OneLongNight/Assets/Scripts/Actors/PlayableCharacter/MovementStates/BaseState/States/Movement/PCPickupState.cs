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

    private const float timerStartTime = 0.25f;
    private float timer;

    public void OnEnterState()
    {
        player.PCAnimator.applyRootMotion = true;

        player.PCAnimator.SetBool("isPickingUp", true);

        timer = timerStartTime;
        PCItemInventoryHandler.Instance.PickUpItem();        
       
    }

    public void OnUpdateState()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            OnExitState(player.idleState);
        }
        
    }

    public void OnExitState(IPlayableCharacterState newState)
    {
        player.HasTargetPosition = false;
        player.HasPickupTarget = false;
        player.PCAnimator.SetBool("isPickingUp", false);
        player.CurrentState = newState;
    }
}
