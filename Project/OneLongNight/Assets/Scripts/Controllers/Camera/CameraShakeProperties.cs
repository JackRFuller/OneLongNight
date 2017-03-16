using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CameraShakeProperties
{
    public float angle;
    public float strength;
    public float speed;
    public float duration;
    [Range(0, 1)]
    public float noisePercent;
    [Range(0, 1)]
    public float dampingPercent;
    [Range(0, 1)]
    public float rotationPercent;
}
