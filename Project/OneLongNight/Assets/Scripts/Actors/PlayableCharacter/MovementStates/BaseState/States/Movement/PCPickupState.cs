using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCPickupState : IPlayableCharacterState
{
    private readonly StatePatternPlayableCharacter player;
    public PCPickupState(StatePatternPlayableCharacter pcStateController)
    {
        player = pcStateController;
    }

    private const float timerStartTime = 0.75f;
    private float timer;

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
