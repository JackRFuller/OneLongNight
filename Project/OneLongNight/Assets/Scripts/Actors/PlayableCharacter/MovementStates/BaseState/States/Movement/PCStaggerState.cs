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
        for(int i = 0; i < player.PCAnimator.parameterCount;i++)
        {
            if(player.PCAnimator.parameters[i].type == AnimatorControllerParameterType.Bool)
            {
                player.PCAnimator.SetBool(player.PCAnimator.parameters[i].name, false);
            }
        }       

        //Rotate To Look At Attacker
        Vector3 lookAtPos = new Vector3(StatePatternPlayableCharacter.attackingEnemy.position.x,
                                        player.transform.position.y,
                                        StatePatternPlayableCharacter.attackingEnemy.position.z);
                

        player.transform.LookAt(lookAtPos);
        player.PCAnimator.applyRootMotion = true;
        //Set Stagger Direction
        //player.PCAnimator.SetFloat("StaggeredDirection", staggeredDirection);

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
            if(player.HasTargetPosition || player.HasPickupTarget || player.EnemyTarget)
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
        player.PCAnimator.SetBool("isStaggered", false);
        player.CurrentState = newState;
    }
}
