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
    private bool hasResetToFollowEnemy;


    public void OnEnterState()
    {
        player.NavAgent.velocity = Vector3.zero;
        player.NavAgent.Resume();
        player.PCAnimator.applyRootMotion = false;
        player.NavAgent.ResetPath();

        //Look AT Target
        if (!player.EnemyTarget)
        {
            player.transform.LookAt(player.TargetPosition);
            player.NavAgent.destination = player.TargetPosition;
            oldPosition = player.TargetPosition;
        }                 
        else
        {
            player.transform.LookAt(player.EnemyTarget);
            player.NavAgent.destination = player.EnemyTarget.position;
            oldPosition = player.EnemyTarget.position;
        }
        
        player.PCAnimator.SetBool("IsMoving",true);
        //player.PCAnimator.SetBool("isBlocking", player.IsBlocking);
       
       
    }

    public void OnUpdateState()
    {
        if(player.EnemyTarget)
        {
            MoveTowardsEnemy();
        }
        else
        {
            MoveTowardsPoint();
        }      
    }

    private void MoveTowardsEnemy()
    {
        if(!hasResetToFollowEnemy)
        {
            player.NavAgent.velocity = Vector3.zero;
            player.NavAgent.ResetPath();
            player.NavAgent.destination = player.EnemyTarget.position;
            player.transform.LookAt(player.EnemyTarget.position);
            oldPosition = player.TargetPosition;
            hasResetToFollowEnemy = true;
        }
        
        player.NavAgent.destination = player.EnemyTarget.position;

        float dist = Vector3.Distance(player.transform.position, player.EnemyTarget.position);
        if (dist < StatePatternPlayableCharacter.WeaponRange)
        {
            player.transform.LookAt(player.EnemyTarget.position);
            player.HasTargetPosition = false;
            OnExitState(player.attackState);
        }
    }

    private void MoveTowardsPoint()
    {
        if (oldPosition != player.TargetPosition)
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
        hasResetToFollowEnemy = false;
        player.NavAgent.Stop();

        player.PCAnimator.SetBool("IsMoving", false);        
        player.CurrentState = newState;
    }
}
