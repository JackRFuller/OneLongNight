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

    Vector2 smoothDeltaPosition = Vector2.zero;
    Vector2 velocity = Vector2.zero;

    public void OnEnterState()
    {
        player.NavAgent.destination = player.TargetMovePosition; 
        player.transform.LookAt(player.TargetMovePosition);
        player.NavAgent.Resume();

        player.Anim.SetBool("IsMoving", true);
        player.Anim.applyRootMotion = false;
    }

    public void OnUpdateState()
    {
        player.NavAgent.destination = player.TargetMovePosition;
        // player.transform.LookAt(player.TargetMovePosition);


        float distToPoint = Vector3.Distance(player.transform.position, player.TargetMovePosition);
        if(distToPoint < 0.5f)
        {
            player.HasTargetMovePosition = false;
            OnExitState(player.idleState);
        }

    }

    

    public void OnExitState(IPlayableCharacterState newState)
    {
        player.CurrentState = newState;
    }
}
