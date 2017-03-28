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
    private float attackCooldownTimer;

    private int attackCount = 0;

    public void OnEnterState()
    {
        //Stop Movement
        enemy.NavAgent.Stop();  
        enemy.NavAgent.velocity = Vector3.zero;
        

        attackCooldownTimer = enemy.AttackCooldownTime;
        attackTimer = 2.1f;
        attackCount = 0;

        enemy.transform.LookAt(enemy.Target);
        enemy.EnemyAnim.SetInteger("AttackCount", attackCount);
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
            if(attackCooldownTimer > 0)
            {
                attackCooldownTimer -= Time.deltaTime;
            }
            else
            {
                //CHeck How Close We Are To The Player
                float distToPC = Vector3.Distance(enemy.transform.position, enemy.Target.position);

                if (distToPC < enemy.DistanceToPCToAttack)
                {
                    if (enemy.CanAttack())
                    {
                        //Reset Timer
                        attackTimer = 2.1f;
                        attackCooldownTimer = enemy.AttackCooldownTime;

                        //increment Attack Count
                        attackCount++;
                        if (attackCount > 1)
                            attackCount = 0;
                        enemy.transform.LookAt(enemy.Target);
                        enemy.EnemyAnim.SetInteger("AttackCount", attackCount);

                    }
                    else
                    {
                        //Start Moving
                        OnExitState(enemy.moveState);
                    }

                }
                else
                {
                    //Start Moving
                    OnExitState(enemy.moveState);

                }
            }
        }
    }

    public void OnExitState(IEnemyState newState)
    {
        enemy.EnemyAnim.SetBool("isAttacking", false);
        enemy.CurrentState = newState;
    }
}
