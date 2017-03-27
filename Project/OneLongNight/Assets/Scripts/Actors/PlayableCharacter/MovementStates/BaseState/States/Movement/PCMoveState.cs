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

    private Vector3 oldPosition;


    public void OnEnterState()
    {
        player.PCAnimator.applyRootMotion = false;
        player.NavAgent.ResetPath();

        //Look AT Target
        player.transform.LookAt(player.TargetPosition);

        player.NavAgent.velocity = Vector3.zero;
        player.NavAgent.destination = player.TargetPosition;
        player.NavAgent.Resume();

        
        player.PCAnimator.SetBool("IsMoving",true);
        player.PCAnimator.SetBool("isBlocking", player.IsBlocking);
       
        oldPosition = player.TargetPosition;
    }

    public void OnUpdateState()
    {
        if(oldPosition != player.TargetPosition)
        {
            player.NavAgent.velocity = Vector3.zero;
            player.NavAgent.ResetPath();
            player.NavAgent.destination = player.TargetPosition;
            player.transform.LookAt(player.TargetPosition);
            oldPosition = player.TargetPosition;
        }
       

        float dist = Vector3.Distance(player.transform.position, player.TargetPosition);
        if (dist < 0.2f)
        {
            player.HasTargetPosition = false;
            OnExitState(player.idleState);
        }
    }

    public void OnExitState(IPlayableCharacterState newState)
    {
        player.NavAgent.Stop();

        player.PCAnimator.SetBool("IsMoving", false);        
        player.CurrentState = newState;
    }
}
