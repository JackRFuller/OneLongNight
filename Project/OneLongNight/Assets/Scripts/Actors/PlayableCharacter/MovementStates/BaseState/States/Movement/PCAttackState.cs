using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PCAttackState : IPlayableCharacterState
{

    private readonly StatePatternPlayableCharacter player;
    public PCAttackState(StatePatternPlayableCharacter pcStateController)
    {
        player = pcStateController;
    }

    
    
    public void OnEnterState()
    {
        

    }

    public void OnUpdateState()
    {
       
    }

    private void QueueLightAttacks()
    {
        
    }

    private void QueueMediumAttacks()
    {
       
    }

    private void QueueHeavyAttacks()
    {
       
    }

    private void CheckForAttackInput()
    {
        
    }

    //Called from the script on the animation state
    public static void InitiateAttack(float animationLength)
    {
        
    }

    public void OnExitState(IPlayableCharacterState newState)
    {
        
    }

	
}
