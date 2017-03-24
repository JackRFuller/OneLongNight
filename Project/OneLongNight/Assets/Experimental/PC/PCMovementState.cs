using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCMovementState : IPlayableCharacterState
{
    private readonly PCStateController player;
    public PCMovementState(PCStateController pcStateController)
    {
        player = pcStateController;
    }

    Vector3 lookDirection = Vector3.zero;

    float timestamp = 0;
    float cooldownPeriod = 0.1f;

    Quaternion oldRot;
    Vector3 lastVector;
    private bool hasRecordedLastRot;


    public void OnEnterState()
    {
       
    }

    public void OnUpdateState()
    {
         

        //Get Input
        Vector3 movementDirection = player.MovementVector;
        
        movementDirection = player.transform.TransformDirection(movementDirection);

        movementDirection *= 3f;

        

        player.Controller.Move(movementDirection * Time.deltaTime);

        if (player.MovementVector != Vector3.zero)
            player.transform.rotation = Quaternion.LookRotation(player.MovementVector);


    }

    public void OnExitState(IPlayableCharacterState newState)
    {

    }
}
