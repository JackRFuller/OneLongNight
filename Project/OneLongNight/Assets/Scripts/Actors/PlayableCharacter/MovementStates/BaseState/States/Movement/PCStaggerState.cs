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
