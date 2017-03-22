using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSmoothLookAt : MonoBehaviour
{
    public Transform target;
    public float damping = 6.0f;
    public bool isSmooth = true;


    private void LateUpdate()
    {
        if (target)
        {
            if (isSmooth)
            {
                Quaternion rotation = Quaternion.LookRotation(target.position - transform.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
            }
            else
            {
                transform.LookAt(target);
            }
        }
    }
}
