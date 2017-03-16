﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScreenShake : MonoSingleton<CameraScreenShake>
{

    [SerializeField]
    private PCCameraController cameraController;

    private const float maxAngle = 10f;
    public CameraShakeProperties testProperties;
    private IEnumerator currentShakeCorountine;
   
    public void TestShake()
    {
        StartShake(testProperties);
    }

    public void StartShake(CameraShakeProperties properties)
    {
        if (currentShakeCorountine != null)
        {
            StopCoroutine(currentShakeCorountine);
        }

        currentShakeCorountine = Shake(properties);
        StartCoroutine(currentShakeCorountine);
    }

    IEnumerator Shake(CameraShakeProperties properties)
    {
        cameraController.TurnCameraFollowOff();
        

        float completionPercent = 0;
        float movePercent = 0;

        float angle_radians = properties.angle * Mathf.Deg2Rad - Mathf.PI;
        Vector3 previousWaypoint = Vector3.zero;
        Vector3 currentWaypoint = Vector3.zero;
        float moveDistance = 0;

        Quaternion targetRot = Quaternion.identity;
        Quaternion previousRotation = Quaternion.identity;

        do
        {
            if (movePercent >= 1 || completionPercent == 0)
            {
                float dampingFactor = DampingCurve(completionPercent, properties.dampingPercent);

                float noiseAngle = (Random.value - .5f) * Mathf.PI;
                angle_radians += Mathf.PI * noiseAngle * properties.noisePercent;
                currentWaypoint = new Vector3(Mathf.Cos(angle_radians), Mathf.Sin(angle_radians)) * properties.strength * dampingFactor;
                previousWaypoint = transform.localPosition;
                moveDistance = Vector3.Distance(currentWaypoint, previousWaypoint);

                targetRot = Quaternion.Euler(new Vector3(currentWaypoint.y, currentWaypoint.x).normalized * properties.rotationPercent * dampingFactor * maxAngle);
                previousRotation = transform.localRotation;
                movePercent = 0;
            }

            completionPercent += Time.deltaTime / properties.duration;
            movePercent += Time.deltaTime / movePercent * properties.speed;
            transform.localPosition = Vector3.Lerp(previousWaypoint, currentWaypoint, movePercent);
            transform.localRotation = Quaternion.Slerp(previousRotation, targetRot, movePercent);


            yield return null;
        }
        while (moveDistance > 0);

        cameraController.TurnCameraFollowOn();
    }

    float DampingCurve(float x, float dampingPercent)
    {
        x = Mathf.Clamp01(x);
        float a = Mathf.Lerp(2, .25f, dampingPercent);
        float b = 1 - Mathf.Pow(x, a);
        return b * b * b;
    }

}
