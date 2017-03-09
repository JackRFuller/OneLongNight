using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PCAttackState : IPlayableCharacterState
{

    private readonly StatePatternPlayableCharacter player;
    public PCAttackState(StatePatternPlayableCharacter pcStateController)
    {
        player = pcStateController;
    }

    private ItemData currentWeapon;

    private bool hasQueuedNextAttack; //Determins whether the player has put in input to attack again
    private int attackCount; //Used to track the index of the attack

    public static bool CanTakeAttackInput;
    public static bool CanTakeMovementInput;

    private bool hasInitiatedNextAttack;

    public static float animationTimer;


    public void OnEnterState()
    {
        attackCount = 0;

        currentWeapon = PCItemInventoryHandler.CurrentWeapon;
        player.PCAnimator.SetBool("isAttacking", true);
        player.PCAnimator.SetInteger("AttackCount", attackCount);        
    }

    public void OnUpdateState()
    {
        if(CanTakeAttackInput)
            CheckForAttackInput();

        if(animationTimer > 0)
        {
            animationTimer -= Time.deltaTime;

            if (hasQueuedNextAttack)
            {
                //player.PCAnimator.SetBool("isAttacking", true);

                //First Check if we're at the end of the attack runway
                switch (currentWeapon.weaponWeight)
                {
                    case ItemData.WeaponWeight.Light:

                        QueueLightAttacks();

                        break;

                    case ItemData.WeaponWeight.Medium:

                        if (attackCount < 3)
                        {
                            QueueMediumAttacks();
                        }

                        else if (attackCount == 3)
                        {
                            DetectMovement();
                        }
                        break;

                    case ItemData.WeaponWeight.Heavy:

                        if (attackCount < 2)
                        {
                            QueueHeavyAttacks();
                        }
                        else
                        {
                            DetectMovement();
                        }
                        break;
                }
            }
        }
        else
        {
            DetectMovement();
        }
    }

    private void DetectMovement()
    {
        //Check if We're Getting Movement
        if(player.MovementVector != Vector3.zero)
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

    private void QueueLightAttacks()
    {
        attackCount++;
        if (attackCount > 1)
            attackCount = 0;

        player.PCAnimator.SetInteger("AttackCount", attackCount);

        hasQueuedNextAttack = false;
        CanTakeAttackInput = false;
        hasInitiatedNextAttack = true;
    }

    private void QueueMediumAttacks()
    {
        attackCount++;
        player.PCAnimator.SetInteger("AttackCount", attackCount);

        hasQueuedNextAttack = false;
        CanTakeAttackInput = false;
        hasInitiatedNextAttack = true;
    }

    private void QueueHeavyAttacks()
    {
        attackCount++;
        player.PCAnimator.SetInteger("AttackCount", attackCount);

        hasQueuedNextAttack = false;
        CanTakeAttackInput = false;
        hasInitiatedNextAttack = true;
    }

    private void CheckForAttackInput()
    {

        Player playerControls = ReInput.players.GetPlayer(0);

        if (playerControls.GetButtonDown("Attack") || Input.GetMouseButtonDown(0
            ))
        {
            hasQueuedNextAttack = true;
            CanTakeMovementInput = false;
        }
    }

    public void OnExitState(IPlayableCharacterState newState)
    {
        player.PCAnimator.SetBool("isAttacking", false);
        Debug.Log("Moved to " + newState);
        player.CurrentState = newState;
    }

	
}
