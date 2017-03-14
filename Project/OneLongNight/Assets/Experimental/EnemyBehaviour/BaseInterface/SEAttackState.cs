using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEAttackState : IEnemyState
{
    private readonly StatePatternStandardEnemy enemy;
    public SEAttackState(StatePatternStandardEnemy enemyStateController)
    {
        enemy = enemyStateController;
    }

    private float attackTimer;

    public void OnEnterState()
    {
        attackTimer = 2.1f;
        enemy.EnemyAnim.SetBool("isAttacking", true);
    }

    public void OnUpdateState()
    {
        //Run The Attack
        if(attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
        else
        {
            //CHeck How Close We Are To The Player
            float distToPC = Vector3.Distance(enemy.transform.position, enemy.Target.position);

            if(distToPC < enemy.DistanceToPCToAttack)
            {
                //Reset Timer
                attackTimer = 2.1f;
            }
            else
            {
                //Start Moving
                OnExitState(enemy.moveState);

            }
        }
    }

    public void OnExitState(IEnemyState newState)
    {
        enemy.EnemyAnim.SetBool("isAttacking", false);

        enemy.CurrentState = newState;
    }
}
