using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayableCharacterState
{
    void OnEnterState();

    void OnUpdateState();

    void OnExitState(IPlayableCharacterState newState);
	
}
