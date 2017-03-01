using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCBlockIdleState : IPlayableCharacterState
{
    private readonly StatePatternPlayableCharacter player;
    public PCBlockIdleState(StatePatternPlayableCharacter pcStateController)
    {
        player = pcStateController;
    }

    public void OnEnterState()
    {

    }

    public void OnUpdateState()
    {
        //Check For Roll
        if (player.IsRolling)
        {
            //Check We Have Enough Stamina
            if (PCAttributes.Instance.CheckIfPCHasEnoughStamina(player.RollAction.ActionCost))
            {
                player.PCAnimator.SetBool("isRolling", true);
                OnExitState(player.rollState);
            }
        }        

        //Check for Movement
        if (player.MovementVector != Vector3.zero)
        {
            //CHeck if We're Blocking
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
            if(!player.IsBlocking)
            {
                player.PCAnimator.SetBool("isBlocking", false);
                OnExitState(player.idleState);
            }
        }
    }

    public void OnExitState(IPlayableCharacterState newState)
    {
        player.CurrentState = newState;
    }

}
