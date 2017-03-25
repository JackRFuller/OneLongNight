
// =================================	
// Namespaces.
// =================================

using UnityEngine;
//using System.Collections;

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

        //[RequireComponent(typeof(TrailRenderer))]

        public class FollowTargetRotation : MonoBehaviour
        {
            // =================================	
            // Nested classes and structures.
            // =================================

            // ...

            // =================================	
            // Variables.
            // =================================

            // ...

            public Transform target;

            // ...

            public float speed = Mathf.Infinity;

            public Space relativeTo = Space.Self;
            public Space targetRelativeTo = Space.World;

            public UpdateCallback updateCallback = UpdateCallback.fixedUpdate;

            public bool followX = true;
            public bool followY = true;
            public bool followZ = true;

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

            }

            // ...

            void update()
            {
                Vector3 eulerRotation;

                Vector3 targetEulerRotation = 
                    (targetRelativeTo == Space.World ? target.eulerAngles : target.localEulerAngles);

                if (relativeTo == Space.Self)
                {
                    eulerRotation = transform.localEulerAngles;

                    if (followX)
                    {
                        eulerRotation.x = Mathf.LerpAngle(eulerRotation.x, targetEulerRotation.x, Time.deltaTime * speed);
                    }
                    if (followY)
                    {
                        eulerRotation.y = Mathf.LerpAngle(eulerRotation.y, targetEulerRotation.y, Time.deltaTime * speed);
                    }
                    if (followZ)
                    {
                        eulerRotation.z = Mathf.LerpAngle(eulerRotation.z, targetEulerRotation.z, Time.deltaTime * speed);
                    }

                    transform.localEulerAngles = eulerRotation;
                }
                else
                {
                    eulerRotation = transform.eulerAngles;

                    if (followX)
                    {
                        eulerRotation.x = Mathf.LerpAngle(eulerRotation.x, targetEulerRotation.x, Time.deltaTime * speed);
                    }
                    if (followY)
                    {
                        eulerRotation.y = Mathf.LerpAngle(eulerRotation.y, targetEulerRotation.y, Time.deltaTime * speed);
                    }
                    if (followZ)
                    {
                        eulerRotation.z = Mathf.LerpAngle(eulerRotation.z, targetEulerRotation.z, Time.deltaTime * speed);
                    }

                    transform.eulerAngles = eulerRotation;
                }
            }

            // ...

            void Update()
            {
                if (updateCallback == UpdateCallback.update)
                {
                    update();
                }
            }

            // ...

            void FixedUpdate()
            {
                if (updateCallback == UpdateCallback.fixedUpdate)
                {
                    update();
                }
            }

            // ...

            void LateUpdate()
            {
                if (updateCallback == UpdateCallback.lateUpdate)
                {
                    update();
                }
            }

            // ...

            public void reset()
            {

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
