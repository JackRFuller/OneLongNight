using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCRollState : IPlayableCharacterState
{
    private readonly StatePatternPlayableCharacter player;
    public PCRollState(StatePatternPlayableCharacter pcStateController)
    {
        player = pcStateController;
    }

    private const float timerStartTime = 1.1f;
    private float timer;

    public void OnEnterState()
    {
        player.HasTargetPosition = false;
        player.NavAgent.Stop();

        //Look At Destination
        player.transform.LookAt(player.TargetPosition);

        player.PCAnimator.applyRootMotion = true;
        player.PCAnimator.SetBool("isRolling", true);
        PCAttributes.Instance.RemoveStamina(player.RollAction.ActionCost);
        timer = timerStartTime;
    }

    public void OnUpdateState()
    {
        if(timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            OnExitState(player.idleState);
        }
    }

    public void OnExitState(IPlayableCharacterState newState)
    {
        player.PCAnimator.applyRootMotion = false;
        player.NavAgent.velocity = Vector3.zero;        
        player.PCAnimator.SetBool("isRolling", false);
        player.CurrentState = newState;
    }


}
