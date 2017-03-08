using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Director;

public class AnimationCombatStateHandler : StateMachineBehaviour
{
    

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PCAttackState.CanTakeAttackInput = true;
        PCAttackState.animationTimer = stateInfo.length;
    }
}
