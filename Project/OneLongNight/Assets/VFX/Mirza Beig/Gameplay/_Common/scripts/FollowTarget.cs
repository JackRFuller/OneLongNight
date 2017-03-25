
// =================================	
// Namespaces.
// =================================

using UnityEngine;
using System.Collections;

using MirzaBeig.Common;

// =================================	
// Define namespace.
// =================================

namespace MirzaBeig
{

    namespace Gameplay
    {

        // =================================	
        // Classes.
        // =================================

        //[ExecuteInEditMode]
        [System.Serializable]

        //[RequireComponent(typeof(Rigidbody))]

        public class FollowTarget : MonoBehaviour
        {
            // =================================	
            // Nested classes and structures.
            // =================================

            // ...

            // =================================	
            // Variables.
            // =================================

            // Required. The target to follow.

            public Transform target;

            // Optional, but don't try to use target's velocity.

            public Rigidbody targetRigidbody;

            // Position and rotation update speed.

            public float moveSpeed = 4.0f;

            public float turnSpeed = 8.0f; // Up/down, left/right (rotation around X and Y).
            public float rollSpeed = 8.0f; // Spin (rotation around Z).

            // Roll up direction. Used for controlling rolls seperate from turning.

            Vector3 rollUp = Vector3.up;

            // Used if smoothDampPosition is true.

            public float smoothDampMoveSpeed = 15.0f;
            //public float rotationSmoothDampTime = 0.01f;

            // Update methods.

            public UpdateCallback positionUpdateCallback = UpdateCallback.fixedUpdate;
            public UpdateCallback rotationUpdateCallback = UpdateCallback.lateUpdate;

            // Substitute lerp with smoothDamp function.

            public bool smoothDampPosition = false;

            // Enable/disable following target's position and rotation.

            public bool _updatePosition = true;
            public bool _updateRotation = true;

            // Tilt following can be disabled.

            public bool followTargetTilt = true;

            // Use target's local parameters?

            public bool followTargetLocalPosition = false;
            public bool followTargetLocalRotation = false;

            // Follow target's heading as rotation.
            // Must be > min else uses target's transform forward.

            public bool useTargetVelocityAsRotation = false;
            public float minTargetVelocity = 4.0f;

            // =================================	
            // Functions.
            // =================================

            // ...

            void Awake()
            {

            }

            // ...

            void Start()
            {
                //rollUp = target.up;
            }

            // ...

            void updatePosition()
            {
                Vector3 targetPosition =
                    followTargetLocalPosition ? target.localPosition : target.position;

                if (Application.isPlaying)
                {

                    if (!smoothDampPosition)
                    {
                        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * moveSpeed);
                    }
                    else
                    {
                        Vector3 smoothDampVelocity = Vector3.zero;
                        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref smoothDampVelocity, 1.0f / smoothDampMoveSpeed);
                    }
                }
                else
                {
                    transform.position = targetPosition;
                }
            }
            void updateRotation()
            {
                if (Application.isPlaying)
                {
                    Quaternion targetRotation;

                    Vector3 targetUp;
                    Vector3 targetForward;

                    if (useTargetVelocityAsRotation)
                    {
                        Vector3 targetVelocityDirection = targetRigidbody.velocity.normalized;

                        if (targetVelocityDirection.magnitude < minTargetVelocity)
                        {
                            targetForward = target.forward;
                        }
                        else
                        {
                            targetForward = targetVelocityDirection;
                        }

                        targetUp = Vector3.up;
                    }
                    else
                    {
                        targetUp = target.up;
                        targetForward = (followTargetLocalRotation ? target.localRotation : target.rotation) * Vector3.forward;
                    }

                    if (!followTargetTilt)
                    {
                        targetForward.y = 0.0f;

                        if (targetForward.sqrMagnitude < float.Epsilon)
                        {
                            targetForward = transform.forward;
                        }
                    }

                    targetRotation = Quaternion.LookRotation(targetForward, rollUp);
                    rollUp = rollSpeed > 0.0f ? Vector3.Slerp(rollUp, targetUp, rollSpeed * Time.deltaTime) : Vector3.up;

                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
                }
                else
                {
                    transform.rotation = followTargetLocalRotation ? target.localRotation : target.rotation;
                }
            }

            // ...

            void Update()
            {
                if (_updatePosition)
                {
                    if (positionUpdateCallback == UpdateCallback.update)
                    {
                        updatePosition();
                    }
                }
                if (_updateRotation)
                {
                    if (rotationUpdateCallback == UpdateCallback.update)
                    {
                        updateRotation();
                    }
                }
            }

            // ...

            void FixedUpdate()
            {
                if (_updatePosition)
                {
                    if (positionUpdateCallback == UpdateCallback.fixedUpdate)
                    {
                        updatePosition();
                    }
                }
                if (_updateRotation)
                {
                    if (rotationUpdateCallback == UpdateCallback.fixedUpdate)
                    {
                        updateRotation();
                    }
                }
            }

            // ...

            void LateUpdate()
            {
                if (_updatePosition)
                {
                    if (positionUpdateCallback == UpdateCallback.lateUpdate)
                    {
                        updatePosition();
                    }
                }
                if (_updateRotation)
                {
                    if (rotationUpdateCallback == UpdateCallback.lateUpdate)
                    {
                        updateRotation();
                    }
                }
            }

            // =================================	
            // End functions.
            // =================================

        }

        // =================================	
        // End namespace.
        // =================================

    }

}

// =================================	
// --END-- //
// =================================
