using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCLookAt : BaseMonoBehaviour
{
    private Animator animator;

    [SerializeField]
    private Transform head;
    public Vector3 lookAtTargetPosition;
    private float lookAtCoolTime = 0.2f;
    private float lookAtHeatTIme = 0.2f;
    private bool looking = true;

    private Vector3 lookAtPosition;

    private float lookAtWeight;

    private void Start()
    {
        if(!head)
        {
            Debug.LogError("No head transform - LookAt Disabled");
            enabled = false;
            return;
        }

        animator = this.GetComponent<Animator>();
        lookAtTargetPosition = head.position + transform.forward;
        lookAtPosition = lookAtTargetPosition;
    }

    void OnAnimatorIK()
    {
        lookAtTargetPosition.y = head.position.y;
        float lookAtTargetWeight = looking ? 1.0f : 0.0f;

        Vector3 curDir = lookAtPosition - head.position;
        Vector3 futDir = lookAtTargetPosition - head.position;

        curDir = Vector3.RotateTowards(curDir, futDir, 6.28f * Time.deltaTime, float.PositiveInfinity);
        lookAtPosition = head.position + curDir;

        float blendTime = lookAtTargetWeight > lookAtWeight ? lookAtHeatTIme : lookAtCoolTime;
        lookAtWeight = Mathf.MoveTowards(lookAtWeight, lookAtTargetWeight, Time.deltaTime / blendTime);
        animator.SetLookAtWeight(lookAtWeight, 0.2f, 0.5f, 0.7f, 0.5f);
        animator.SetLookAtPosition(lookAtPosition);
    }


}
