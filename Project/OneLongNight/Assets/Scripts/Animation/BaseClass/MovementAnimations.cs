using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Movement Animation", menuName = "Scriptable Object/Movement Anims", order = 1)]
public class MovementAnimations : ScriptableObject
{
    //Movement
	public AnimationClipData idleAnim;
    public AnimationClipData moveAnim;
    public AnimationClipData rollAnim;

    public AnimationClipData blockingMoveAnim;
    public AnimationClipData blockingIdle;
}
