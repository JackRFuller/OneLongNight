using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : StateMachineBehaviour
{
    public float windUpSpeed;
    public float windUpTime;
    private float windUpTimer;

    public float attackSpeed;
    public float attackPauseTime;
    private float attackPauseTimer;
    

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        windUpTimer = windUpTime;
        attackPauseTimer = attackPauseTime;
        animator.SetFloat("AttackSpeed", windUpSpeed);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(windUpTimer > 0)
        {
            windUpTimer -= Time.deltaTime;
        }
        else
        {
            animator.SetFloat("AttackSpeed",0.25f);

            if (attackPauseTimer > 0)
            {
                attackPauseTimer -= Time.deltaTime;
            }
            else
            {
                animator.SetFloat("AttackSpeed", attackSpeed);
            }

        }
    }





}
