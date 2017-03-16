using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCStaggerState : IPlayableCharacterState
{
    private readonly StatePatternPlayableCharacter player;
    public PCStaggerState(StatePatternPlayableCharacter pcStateController)
    {
        player = pcStateController;
    }

    private float staggerCooldownTimer;

    public void OnEnterState()
    {
        //Turn Off Other Anim States
        player.PCAnimator.SetBool("isRolling", false);
        player.PCAnimator.SetBool("isAttacking", false);
        player.PCAnimator.SetBool("isBlocking", false);
        player.PCAnimator.SetBool("isPickingUp", false);

        //Rotate To Look At Attacker
        Vector3 lookAtPos = new Vector3(StatePatternPlayableCharacter.attackingEnemy.position.x,
                                        player.transform.position.y,
                                        StatePatternPlayableCharacter.attackingEnemy.position.z);
                

        player.transform.LookAt(lookAtPos);

        Vector3 rotation = player.transform.eulerAngles;

        rotation = new Vector3(rotation.x, rotation.y - 45, rotation.z);

        player.transform.eulerAngles = rotation;

        //Work Out Direction of Hit3
        Vector3 relativehit = StatePatternPlayableCharacter.hitDirection;

        //Determine Hit Direction            
        relativehit = player.transform.InverseTransformPoint(relativehit);

        int staggeredDirection = 0;

        if (relativehit.x < 0)
            staggeredDirection = 0;

        if (relativehit.x >= 0)
            staggeredDirection = 1;

        //Set Stagger Direction
        player.PCAnimator.SetFloat("StaggeredDirection", staggeredDirection);

        //Enable Stagger State
        player.PCAnimator.SetBool("isStaggered", true);

        //Set StaggerCooldown
        staggerCooldownTimer = player.StaggerCooldown;
    }

    public void OnUpdateState()
    {
        if(staggerCooldownTimer >= 0f)
        {
            staggerCooldownTimer -= Time.deltaTime;
        }
        else
        {
            //Check for Attack
            if (player.IsAttacking)
            {
                //Check We Have Enough Stamina
                if (PCAttributes.Instance.CheckIfPCHasEnoughStamina(PCItemInventoryHandler.CurrentWeapon.weaponAttackCosts[0]))
                {
                    //Check if We Have Enough Stamina to Attack
                    OnExitState(player.attackState);
                }
            }

            //Check if We're Picking Up Items
            if (player.IsPickingUp)
            {
                player.PCAnimator.SetBool("isPickingUp", true);
                OnExitState(player.pickUpState);
            }
            else
            {
                //Check we're not standing still
                if (player.MovementVector != Vector3.zero)
                {
                    player.PCAnimator.SetInteger("Movement", 1);

                    //Check if We're Blocking
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
                    player.PCAnimator.SetInteger("Movement", 0);

                    //Check if We're Blocking
                    if (player.IsBlocking)
                    {
                        player.PCAnimator.SetBool("isBlocking", true);
                        OnExitState(player.blockIdleState);
                    }
                    else
                    {
                        player.PCAnimator.SetBool("isBlocking", false);
                        OnExitState(player.idleState);
                    }
                }
            }
           
        }
    }

    public void OnExitState(IPlayableCharacterState newState)
    {
        player.PCAnimator.SetBool("isStaggered", false);
        player.CurrentState = newState;
    }
}
