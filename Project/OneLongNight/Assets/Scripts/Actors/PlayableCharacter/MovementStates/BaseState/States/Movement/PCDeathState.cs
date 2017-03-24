using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCDeathState : IPlayableCharacterState
{
    private readonly StatePatternPlayableCharacter player;
    public PCDeathState(StatePatternPlayableCharacter pcStateController)
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
