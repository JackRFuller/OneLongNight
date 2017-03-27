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
        player.NavAgent.velocity = Vector3.zero;
        player.NavAgent.ResetPath();
        player.NavAgent.Stop();

        player.PCAnimator.applyRootMotion = true;        
        player.PCAnimator.SetBool("IsMoving", false);
        player.PCAnimator.SetBool("isBlocking", player.IsBlocking);
    }

    public void OnUpdateState()
    {
        if(player.IsAttacking)
        {
            //Check We Have Enough Stamina
            if(PCAttributes.Instance.CheckIfPCHasEnoughStamina(PCItemInventoryHandler.CurrentWeapon.weaponAttackCosts[0]))
            {
                //Check if We Have Enough Stamina to Attack
                OnExitState(player.attackState);
            }
        }        

        //Check if Player is Picking Up An Item
        if(player.IsPickingUp)
        {
            OnExitState(player.pickUpState);
        }

        if(player.HasTargetPosition)
        {
            OnExitState(player.moveState);
        }
        else
        {
            if(player.HasPickupTarget)
            {
                OnExitState(player.pickUpState);
            }
        }

        

        ////Check for Movement
        //if (player.MovementVector != Vector3.zero)
        //{
        //    OnExitState(player.moveState);
        //}
        //else
        //{
        //    player.PCAnimator.SetBool("isBlocking", player.IsBlocking);
        //}
    }

    public void OnExitState(IPlayableCharacterState newState)
    {
        player.CurrentState = newState;
    }
}
