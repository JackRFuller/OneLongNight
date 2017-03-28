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

    private static float animationTimer = 1;   

    //Weapon Variables
    private ItemData currentWeapon;
    private static float[] weaponAttackCosts;
    private int weaponDurability;

    public void OnEnterState()
    {
        animationTimer = 1;
        weaponDurability = PCItemInventoryHandler.WeaponDurability;
        player.transform.LookAt(player.EnemyTarget);

        player.PCAnimator.applyRootMotion = false;
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
        if (CanTakeAttackInput)
            CheckForAttackInput();

        if (animationTimer > 0)
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
        if (player.HasTargetPosition || player.HasPickupTarget)
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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            if(Input.GetMouseButton(0))
            {
                if (hit.collider.tag == "Enemy")
                {
                    if (hit.transform == player.EnemyTarget)
                    {
                        hasQueuedNextAttack = true;
                        player.transform.LookAt(player.EnemyTarget);
                    }
                    else
                    {
                        //CHeck if within range
                        float dist = Vector3.Distance(player.transform.position, hit.transform.position);

                        if(dist <= StatePatternPlayableCharacter.WeaponRange)
                        {
                            player.EnemyTarget = hit.transform;
                            hasQueuedNextAttack = true;
                            player.transform.LookAt(player.EnemyTarget);
                            Debug.Log("Found New Target");
                        }
                    }
                }
            }
            
        }
        //Player playerControls = ReInput.players.GetPlayer(0);

        //if (playerControls.GetButtonDown("Attack") || Input.GetMouseButtonDown(0))                                                                               
        //{
        //    hasQueuedNextAttack = true;            
        //}
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
        player.EnemyTarget = null;
        player.PCAnimator.SetBool("isAttacking", false);
        //Debug.Log("Moved to " + newState);
        player.CurrentState = newState;
    }

	
}
