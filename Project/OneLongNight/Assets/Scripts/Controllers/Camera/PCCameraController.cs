using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCCameraController : BaseMonoBehaviour
{
	[SerializeField]
	private StatePatternPlayableCharacter player;

    public Collider target;
    public Vector3 focusAreaSize;
    private FocusArea focusArea;
    public Vector3 cameraOffset;

    public float lookAheadDst;
    public float lookSmoothTime;

    private float currenLookAheadX;
    private float targetLookAheadX;
    private float lookAheadDirX;
    
    private float smoothLookVelocityX;
    private float smoothVelocityX;

    private float currentLookAheadZ;
    private float targetLookAheadZ;    
    private float lookAheadDirZ;
    private float smoothLookVelocityZ;
    private float smoothVelocityZ;

    private bool lookAheadStoppedX;
    private bool lookAheadStoppedZ;

    private void Start()
    {
        focusArea = new FocusArea(target.bounds, focusAreaSize);
    }

    public override void UpdateLate()
    {
        focusArea.Update(target.bounds);

        float newX = focusArea.center.x - cameraOffset.x;
        float newY = focusArea.center.y - cameraOffset.y;
        float newZ = focusArea.center.z - cameraOffset.z;

        Vector3 focusPosition = new Vector3(newX, newY, newZ);

        if(focusArea.velocity.x != 0)
        {
            lookAheadDirX = Mathf.Sign(focusArea.velocity.x);

			if(Mathf.Sign(player.MovementVector.x) == Mathf.Sign(focusArea.velocity.x) && player.MovementVector.x != 0)
            {
                lookAheadStoppedX = false;
                targetLookAheadX = lookAheadDirX * lookAheadDst;
            }
            else
            {
                if(!lookAheadStoppedX)
                {
                    targetLookAheadX = currenLookAheadX + (lookAheadDirX * lookAheadDst - currenLookAheadX) / 4f;
                    lookAheadStoppedX = true;
                }
                   
            }
        }

        if(focusArea.velocity.z != 0)
        {
            lookAheadDirZ = Mathf.Sign(focusArea.velocity.z);

			if (Mathf.Sign(player.MovementVector.z) == Mathf.Sign(focusArea.velocity.z) && player.MovementVector.z != 0)
            {
                lookAheadStoppedZ = false;
                targetLookAheadZ = lookAheadDirZ * lookAheadDst;
            }
            else
            {
                if (!lookAheadStoppedZ)
                {
                    targetLookAheadZ = currentLookAheadZ + (lookAheadDirZ * lookAheadDst - currentLookAheadZ) / 4f;
                    lookAheadStoppedZ = true;
                }

            }
        }

        
        currenLookAheadX = Mathf.SmoothDamp(currenLookAheadX, targetLookAheadX, ref smoothLookVelocityX, lookSmoothTime);
        currentLookAheadZ = Mathf.SmoothDamp(currentLookAheadZ, targetLookAheadZ, ref smoothLookVelocityZ, lookSmoothTime);

        focusPosition += Vector3.forward * currentLookAheadZ;
        focusPosition += Vector3.right * currenLookAheadX;

        transform.position = focusPosition;
    }

    public void OnDrawGizmos()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawCube(focusArea.center, focusAreaSize);
    }

    struct FocusArea
    {
        public Vector3 center;
        public Vector3 velocity;
        private float left;
        private float right;
        private float front;
        private float back;

        public FocusArea(Bounds targetBounds, Vector3 size)
        {
            left = targetBounds.center.x - size.x / 2;
            right = targetBounds.center.x + size.x / 2;

            front = targetBounds.center.z + size.z / 2;
            back =  targetBounds.center.z - size.z / 2;           

            velocity = Vector3.zero;

            float newX = (left + right) / 2;
            float newZ = (front + back / 2);

            center = new Vector3(newX,0,newZ);
        }

        public void Update(Bounds targetBounds)
        {
            float shiftX = 0;

            if(targetBounds.min.x < left)
            {
                shiftX = targetBounds.min.x - left;                
            }
            else if(targetBounds.max.x > right)
            {
                shiftX = targetBounds.max.x - right;                
            }

            left += shiftX;
            right += shiftX;

            float shiftZ = 0;

            if (targetBounds.min.z < back)
            {
                shiftZ = targetBounds.min.z - back;                
            }
            else if (targetBounds.max.z > front)
            {
                shiftZ = targetBounds.max.z - front;               
            }

            front += shiftZ;
            back += shiftZ;

            float newX = (left + right) / 2;
            float newZ = (front + back) / 2;

            center = new Vector3(newX, 0, newZ);
            velocity = new Vector3(shiftX, 0, shiftZ);
        }
    }
}
