using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCRollState : IPlayableCharacterState
{
    private readonly StatePatternPlayableCharacter player;
    public PCRollState(StatePatternPlayableCharacter pcStateController)
    {
        player = pcStateController;
    }

    private const float timerStartTime = 1.1f;
    private float timer;

    public void OnEnterState()
    {
        timer = timerStartTime;
    }

    public void OnUpdateState()
    {
        
    }

    public void OnExitState(IPlayableCharacterState newState)
    {
       
    }


}
