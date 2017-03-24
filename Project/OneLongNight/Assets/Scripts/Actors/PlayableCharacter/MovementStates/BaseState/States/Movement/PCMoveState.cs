﻿using System.Collections;
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
        player.NavAgent.updatePosition = false;
        player.Anim.SetBool("IsMoving", true);
        player.NavAgent.Resume();
    }

    public void OnUpdateState()
    {
        Vector3 worldDeltaPosition = player.NavAgent.nextPosition - player.transform.position;

        //Map 'World Delta Position' to local space
        float dx = Vector3.Dot(player.transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(player.transform.forward, worldDeltaPosition);

        Vector2 deltaPosition = new Vector2(dx, dy);

        //Low Pass Filter the delta Move
        float smooth = Mathf.Min(1.0f, Time.deltaTime / 0.15f);
        smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

        //Update velcoity if time advances
        if (Time.deltaTime > 1e-5f)
            velocity = smoothDeltaPosition / Time.deltaTime;

        bool shouldMove = velocity.magnitude > 0.5f && player.NavAgent.remainingDistance > player.NavAgent.radius;

        player.Anim.SetFloat("velocityX", velocity.x);
        player.Anim.SetFloat("velocityY", velocity.y);

        if (player.transform.position == player.NavAgent.destination)
        {
            player.HasTargetMovePosition = false;
            OnExitState(player.idleState);
        }

        //Update Look At
        player.LookAt.lookAtTargetPosition = player.NavAgent.steeringTarget + player.transform.forward;

        if (worldDeltaPosition.magnitude > player.NavAgent.radius)
            player.NavAgent.nextPosition = player.transform.position + 0.9f * worldDeltaPosition;

    }

    

    public void OnExitState(IPlayableCharacterState newState)
    {
        player.CurrentState = newState;
    }
}
