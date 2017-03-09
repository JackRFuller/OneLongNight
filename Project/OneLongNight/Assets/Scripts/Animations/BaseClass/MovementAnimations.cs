using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Movement Animation", menuName = "Scriptable Object/Movement Anims", order = 1)]
public class MovementAnimations : ScriptableObject
{
	public AnimationClipData idleAnim;
	public AnimationClipData walkAnim;
	public AnimationClipData runAnim;
	public AnimationClipData shieldRunAnim;
	public AnimationClipData slowRollAnim;
	public AnimationClipData fastRollAnim;
}
