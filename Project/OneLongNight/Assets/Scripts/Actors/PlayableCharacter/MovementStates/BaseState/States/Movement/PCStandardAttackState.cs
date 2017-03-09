using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCStandardAttackState : IPlayableCharacterState
{
	private readonly StatePatternPlayableCharacter player;
	public PCStandardAttackState(StatePatternPlayableCharacter pcStateController)
	{
		player = pcStateController;
	}



	private float timer;

	public void OnEnterState()
	{
		player.PCAnimator.SetBool("isAttacking",true);
	}

	public void OnUpdateState()
	{
		//player.PCAnimator.SetBool("isAttacking",true);
	}

	public void OnExitState(IPlayableCharacterState newState)
	{
		player.CurrentState = newState;
	}

}
