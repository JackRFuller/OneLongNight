using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCTPRollState : IPlayableCharacterState
{
    private readonly PCStateManager player;
    public PCTPRollState(PCStateManager pcStateController)
    {
        player = pcStateController;
    }

    private float rollTimer = 1.0f;

    public void OnEnterState()
    {
        rollTimer = 1.0f;

        for (int i = 0; i < player.PlayerAnim.parameterCount; i++)
        {
            if (player.PlayerAnim.parameters[i].type == AnimatorControllerParameterType.Bool)
            {
                player.PlayerAnim.SetBool(player.PlayerAnim.parameters[i].name, false);
            }
        }

        player.transform.rotation = Quaternion.LookRotation(new Vector3(player.Horizontal, 0, player.Vertical));
        player.PlayerAnim.SetBool("IsRolling", true);
    }

    public void OnUpdateState()
    {
        if(rollTimer > 0)
        {
            rollTimer -= Time.deltaTime;
        }
        else
        {
            //Check for Movement
            if(player.Vertical != 0)
            {
                OnExitState(player.moveState);
            }
            else
            {
                OnExitState(player.idleState);
            }
        }
    }

    public void OnExitState(IPlayableCharacterState newState)
    {
        player.PlayerAnim.SetBool("IsRolling", false);
        player.currentState = newState;
    }
}
