﻿using System.Collections;
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

    }

    public void OnUpdateState()
    {
        //Check For Roll
        if(player.IsRolling)
        {
            //Check We Have Enough Stamina
            if(PCAttributes.Instance.CheckIfPCHasEnoughStamina(player.RollAction.ActionCost))
            {
                player.PCAnimator.SetBool("isRolling", true);
                OnExitState(player.rollState);
            }
        }

        //Check for Movement
        if (player.MovementVector != Vector3.zero)
        {
            if(player.IsBlocking)
            {
                player.PCAnimator.SetBool("isBlocking", true);
                OnExitState(player.blockMoveState);
            }
            else
            {
                OnExitState(player.moveState);
            }
            
        }
        else
        {
            if(player.IsBlocking)
            {
                player.PCAnimator.SetBool("isBlocking", true);
                OnExitState(player.blockIdleState);
            }
        }
    }

    public void OnExitState(IPlayableCharacterState newState)
    {
        player.CurrentState = newState;
    }
}
