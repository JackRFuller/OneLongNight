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

    

    private bool hasQueuedNextAttack; //Determins whether the player has put in input to attack again
    private static int attackCount; //Used to track the index of the attack

    private static bool CanTakeAttackInput;    

    private static float animationTimer;   

    //Weapon Variables
    private ItemData currentWeapon;
    private static float[] weaponAttackCosts;
    private int weaponDurability;

    public void OnEnterState()
    {
        weaponDurability = PCItemInventoryHandler.WeaponDurability;

        player.PCAnimator.SetBool("isAttacking", true);
        attackCount = 0;
        player.PCAnimator.SetInteger("AttackCount", attackCount);

        PCItemInventoryHandler.currentWeaponHandler.StartQueuingAttacks();
        //Shield Overrides the Attack Costs
        if(player.HasShield)
        {
            currentWeapon = PCItemInventoryHandler.CurrentShield;           
        }
        else
        {
            currentWeapon = PCItemInventoryHandler.CurrentWeapon;            
        }
        
        weaponAttackCosts = currentWeapon.weaponAttackCosts;

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
            OnExitState(player.moveState);
        }
        else
        {
           OnExitState(player.idleState);
        }
    }

    private void QueueLightAttacks()
    {
        attackCount++;

        if (attackCount > 1)
            attackCount = 0;

        //Check if We Can Afford Attack Cost
        if(PCAttributes.Instance.CheckIfPCHasEnoughStamina(weaponAttackCosts[attackCount]))
        {
            player.PCAnimator.SetInteger("AttackCount", attackCount);

            hasQueuedNextAttack = false;
            CanTakeAttackInput = false;

            PCItemInventoryHandler.currentWeaponHandler.AddAttackIndexToQueue(attackCount);         
        }       
    }

    private void QueueMediumAttacks()
    {
        attackCount++;
        if(PCAttributes.Instance.CheckIfPCHasEnoughStamina(weaponAttackCosts[attackCount]))
        {
            player.PCAnimator.SetInteger("AttackCount", attackCount);

            hasQueuedNextAttack = false;
            CanTakeAttackInput = false;

            PCItemInventoryHandler.currentWeaponHandler.AddAttackIndexToQueue(attackCount);
        }   
    }

    private void QueueHeavyAttacks()
    {
        attackCount++;

        if(PCAttributes.Instance.CheckIfPCHasEnoughStamina(weaponAttackCosts[attackCount]))
        {
            player.PCAnimator.SetInteger("AttackCount", attackCount);

            hasQueuedNextAttack = false;
            CanTakeAttackInput = false;

            PCItemInventoryHandler.currentWeaponHandler.AddAttackIndexToQueue(attackCount);
        }        
    }

    private void CheckForAttackInput()
    {
        Player playerControls = ReInput.players.GetPlayer(0);

        if (playerControls.GetButtonDown("Attack") || Input.GetMouseButtonDown(0))                                                                               
        {
            hasQueuedNextAttack = true;            
        }
    }

    //Called from the script on the animation state
    public static void InitiateAttack(float animationLength)
    {
        //Remove Stamina for  Attack
        PCAttributes.Instance.RemoveStamina(weaponAttackCosts[attackCount]);

        CanTakeAttackInput = true;
        animationTimer = animationLength;
    }

    public void OnExitState(IPlayableCharacterState newState)
    {
        player.PCAnimator.SetBool("isAttacking", false);
        //Debug.Log("Moved to " + newState);
        player.CurrentState = newState;
    }

	
}
