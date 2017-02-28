using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Movement Action", menuName = "Scriptable Object/Movement Action", order = 1)]
public class MovementActions :  ScriptableObject
{
    [SerializeField]
    private float actionCost;
    public float ActionCost
    {
        get
        {
            return actionCost;
        }
    }

    [SerializeField]
    private AudioClip actionSoundFX;
    public AudioClip ActionSoundFX
    {
        get
        {
            return actionSoundFX;
        }
    }
}
