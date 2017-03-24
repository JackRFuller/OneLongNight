using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCMoveState : IPlayableCharacterState
{
    private readonly StatePatternPlayableCharacter player;
    public PCMoveState(StatePatternPlayableCharacter pcStateController)
    {
        player = pcStateController;
    }

    int frameWait;
    private List<Vector3> movementInputs = new List<Vector3>();

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
