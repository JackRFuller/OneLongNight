using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCMoveState : IPlayableCharacterState
{
    private readonly StatePatternPlayableCharacter player;
    public PCMoveState(StatePatternPlayableCharacter pcStateController)
    {
        player = pcStateController;
    }

    int frameWait;
    private List<Vector3> movementInputs = new List<Vector3>();

    public void OnEnterState()
    {
        player.PCAnimator.SetBool("IsMoving",true);
        player.PCAnimator.SetBool("isBlocking", player.IsBlocking);

        movementInputs.Clear();
        frameWait = 0;
    }

    public void OnUpdateState()
    {
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
            OnExitState(player.pickUpState);
        } 
        else 
        {
            movementInputs.Add(player.MovementVector);
            player.PCAnimator.SetBool("isBlocking", player.IsBlocking);

            //Check we're not standing still
            if (player.MovementVector != Vector3.zero)
            {
                frameWait++;
                if(frameWait == 3)
                {
                    player.transform.rotation = Quaternion.LookRotation(player.MovementVector);
                    frameWait = 0;
                }
                
            }
            else
            {
                if(movementInputs.Count > 3)
                {
                    Vector3 lastInput = movementInputs[movementInputs.Count - 2];
                    //Check if last input was 1.0,0,0 || 0,0,1.0

                    if ((Mathf.Abs(lastInput.x) == 1.0f && lastInput.z == 0) || (lastInput.x == 0 && Mathf.Abs(lastInput.z) == 1.0f))
                    {
                        //Check that the input wasn't 1.0,0,1.0
                        Vector3 secondToLastInput = movementInputs[movementInputs.Count - 3];
                        if (Mathf.Abs(secondToLastInput.x) == 1.0f && Mathf.Abs(secondToLastInput.z) == 1.0f)
                        {
                            //Set To that rot
                            player.transform.rotation = Quaternion.LookRotation(secondToLastInput);
                            Debug.Log("Resetting Rotation");
                        }
                    }
                }

                OnExitState(player.idleState);
            }           
        }
    }

    public void OnExitState(IPlayableCharacterState newState)
    {
        player.PCAnimator.SetBool("IsMoving", false);
        movementInputs.Clear();
        player.CurrentState = newState;
    }
}
