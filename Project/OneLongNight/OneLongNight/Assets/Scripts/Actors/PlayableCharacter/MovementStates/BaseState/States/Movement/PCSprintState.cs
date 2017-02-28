using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCSprintState : IPlayableCharacterState
{

    private readonly StatePatternPlayableCharacter player;
    public PCSprintState(StatePatternPlayableCharacter pcStateController)
    {
        player = pcStateController;
    }

    public void OnEnterState()
    {

    }

    public void OnUpdateState()
    {
        //Check for Roll
        if(player.IsRolling)
        {
            //Check we have enough stamina for movement
            if(PCAttributes.Instance.CheckIfPCHasEnoughStamina(player.FastRolLAction.ActionCost))
            {
                player.PCAnimator.SetBool("isRolling", true);
                OnExitState(player.fastRollState);
            }
        }
        else
        {
            //Check for Attack
            if(player.IsTryingToAttack)
            {
                //Check If We've Got Enough Stamina
                if(PCAttributes.Instance.CheckIfPCHasEnoughStamina(player.CurrentWeapon.SprintAttackStaminaCost))
                {
                    //Check How Many Hands
                    if(player.CurrentWeapon.WeaponType == WeaponType.Type.SingleHanded)
                    {
                        player.PCAnimator.SetBool("isSingleHandedWeapon", true);
                    }
                    else
                    {
                        player.PCAnimator.SetBool("isDoubleHandedWeapon", true);
                    }

                    player.PCAnimator.SetBool("isAttacking", true);
                }
                
            }
            
            //Check for Movement
            if (player.MovementVector != Vector3.zero)
            {
                //Check we are still running
                if (player.IsSprinting)
                {
                    //Check We Have Enough Stamina
                    if (PCAttributes.Instance.CheckIfPCHasEnoughStamina(player.SprintAction.ActionCost))
                    {
                        player.PCAnimator.SetBool("isSprinting", true);
                        player.PCAnimator.SetInteger("Movement", 1);


                        //Check that we're not currently rolling
                        AnimatorStateInfo currentState = player.PCAnimator.GetCurrentAnimatorStateInfo(0);

                       player.transform.rotation = Quaternion.LookRotation(player.MovementVector);
                    }
                    else
                    {
                        player.PCAnimator.SetBool("isSprinting", false);
                        OnExitState(player.walkState);
                    }
                }
                else
                {
                    player.PCAnimator.SetBool("isSprinting", false);
                    OnExitState(player.walkState);
                }
            }
            else
            {
                player.PCAnimator.SetBool("isSprinting", false);
                player.PCAnimator.SetInteger("Movement", 0);

                OnExitState(player.idleState);
            }
        }

        
    }

    public void OnExitState(IPlayableCharacterState newState)
    {
        player.CurrentState = newState;
    }


}
