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

    private const float timerStartTime = 0.6f;
    private float timer;

    public void OnEnterState()
    {
        //Turn Off Other Anim States
        for (int i = 0; i < player.PCAnimator.parameterCount; i++)
        {
            if (player.PCAnimator.parameters[i].type == AnimatorControllerParameterType.Bool)
            {
                player.PCAnimator.SetBool(player.PCAnimator.parameters[i].name, false);
            }
        }

        player.PCAnimator.SetBool("isPickingUp",true);

        timer = timerStartTime;
        Vector3 lookPos = PCItemInventoryHandler.foundItem.transform.position - player.transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);

        player.transform.rotation = rotation;
    }

    public void OnUpdateState()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            //Check for Movement
            if (player.MovementVector != Vector3.zero)
            {
                OnExitState(player.moveState);               
            }
            else
            {
                OnExitState(player.idleState);
            }
        }
    }

    public void OnExitState(IPlayableCharacterState newState)
    {
        player.PCAnimator.SetBool("isPickingUp", false);
        player.CurrentState = newState;
    }
}
