using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon Animation", menuName = "Scriptable Object/Weapon Anim", order = 1)]
public class WeaponAnimations : ScriptableObject
{
	public AnimationClipData attackOneAnim;
	public AnimationClipData attackTwoAnim;
	public AnimationClipData attackThreeAnim;
}
