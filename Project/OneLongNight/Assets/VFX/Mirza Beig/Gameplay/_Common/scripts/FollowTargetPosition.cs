
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

        public class FollowTargetPosition : MonoBehaviour
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
                Vector3 position;
                Vector3 targetPosition = target.position;

                if (relativeTo == Space.Self)
                {
                    position = transform.localPosition;

                    if (followX)
                    {
                        position.x = Mathf.Lerp(position.x, targetPosition.x, Time.deltaTime * speed);
                    }
                    if (followY)
                    {
                        position.y = Mathf.Lerp(position.y, targetPosition.y, Time.deltaTime * speed);
                    }
                    if (followZ)
                    {
                        position.z = Mathf.Lerp(position.z, targetPosition.z, Time.deltaTime * speed);
                    }

                    transform.localPosition = position;
                }
                else
                {
                    position = transform.position;

                    if (followX)
                    {
                        position.x = Mathf.Lerp(position.x, targetPosition.x, Time.deltaTime * speed);
                    }
                    if (followY)
                    {
                        position.y = Mathf.Lerp(position.y, targetPosition.y, Time.deltaTime * speed);
                    }
                    if (followZ)
                    {
                        position.z = Mathf.Lerp(position.z, targetPosition.z, Time.deltaTime * speed);
                    }

                    transform.position = position;
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
