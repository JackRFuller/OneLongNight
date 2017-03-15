using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEMoveState : IEnemyState
{
    private readonly StatePatternStandardEnemy enemy;
    public SEMoveState(StatePatternStandardEnemy enemyStateController)
    {
        enemy = enemyStateController;
    }


    public void OnEnterState()
    {
        //Set Nav Mesh Speed
        enemy.NavAgent.destination = enemy.Target.position;
        enemy.NavAgent.speed = enemy.MovementSpeed;

        //Set Enemy Anim To Move
        enemy.EnemyAnim.SetBool("isMoving", true);
    }

    public void OnUpdateState()
    {
        //Update Nav Mesh Destination
        enemy.NavAgent.destination = enemy.Target.position;

        //Check Distance To PC
        float distToPC = Vector3.Distance(enemy.transform.position, enemy.Target.position);

        //If Within Range Then Attack
        if (distToPC < enemy.DistanceToPCToAttack)
        {
            //Check if Enemy is Within Line of Sight
            if(enemy.CanAttack())
            {
                //Move To Attack
                OnExitState(enemy.attackState);
            }
        }
    }
    

    public void OnExitState(IEnemyState newState)
    {
        //Turn Off moevement
        enemy.NavAgent.speed = 0;
        enemy.EnemyAnim.SetBool("isMoving", false);

        enemy.CurrentState = newState;

    }
}
