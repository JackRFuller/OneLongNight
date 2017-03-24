using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCIdleState : IPlayableCharacterState
{
    private readonly StatePatternPlayableCharacter player;
    public PCIdleState(StatePatternPlayableCharacter pcStateController)
    {
        player = pcStateController;
    }

    public void OnEnterState()
    {
      
    }

    public void OnUpdateState()
    {
        
    }

    public void OnExitState(IPlayableCharacterState newState)
    {
       
    }
}
