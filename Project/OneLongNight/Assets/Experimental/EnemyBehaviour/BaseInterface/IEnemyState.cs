using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyState
{
    void OnEnterState();

    void OnUpdateState();

    void OnExitState(IEnemyState newState);
	
}
