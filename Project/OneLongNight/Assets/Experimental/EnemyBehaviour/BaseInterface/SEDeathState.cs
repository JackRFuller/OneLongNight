using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEDeathState : IEnemyState
{
    private readonly StatePatternStandardEnemy enemy;
    public SEDeathState(StatePatternStandardEnemy enemyStateController)
    {
        enemy = enemyStateController;
    }
    private float deathTimer = 1.5f;

    public void OnEnterState()
    {
        enemy.EnemyAnim.SetBool("isDead", true);
        deathTimer = 1.5f;
    }

    public void OnUpdateState()
    {
        deathTimer -= Time.deltaTime;

        if(deathTimer <= 0)
        {
            enemy.gameObject.SetActive(false);
        }
    }

    public void OnExitState(IEnemyState newState)
    {

    }
    
}
