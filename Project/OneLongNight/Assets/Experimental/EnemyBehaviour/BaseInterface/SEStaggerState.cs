using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEStaggerState : IEnemyState
{
    private readonly StatePatternStandardEnemy enemy;
    public SEStaggerState(StatePatternStandardEnemy enemyStateController)
    {
        enemy = enemyStateController;
    }

    private float staggerTime = 1.5f;

    public void OnEnterState()
    {
        //Turn off Previous ANimations
        enemy.EnemyAnim.SetBool("isAttacking", false);
        enemy.EnemyAnim.SetBool("isMoving", false);

        //Turn on Staggering
        enemy.EnemyAnim.SetFloat("Stagger", enemy.StaggeredDirection);        
        enemy.EnemyAnim.SetBool("isStaggered", true);

        staggerTime = 1f;
    }

    public void OnUpdateState()
    {
        if(staggerTime > 0)
        {
            staggerTime -= Time.deltaTime;
        }
        else
        {
            //CHeck How Close We Are To The Player
            float distToPC = Vector3.Distance(enemy.transform.position, enemy.Target.position);

            //Can We Attack?
            if (distToPC < enemy.DistanceToPCToAttack)
            {
                if (enemy.CanAttack())
                {
                    OnExitState(enemy.attackState);
                }
                else
                {
                    //Start Moving
                    OnExitState(enemy.moveState);
                }
            }
            else
            {
                OnExitState(enemy.moveState);
            }
        }
    }

    public void OnExitState(IEnemyState newState)
    {
        enemy.EnemyAnim.SetBool("isStaggered", false);
        enemy.CurrentState = newState;
    }
	
}
