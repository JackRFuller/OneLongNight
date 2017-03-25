
// =================================	
// Namespaces.
// =================================

using UnityEngine;
//using System.Collections;

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

        public class MouseAutoRotateCamera : MonoBehaviour
        {
            // =================================	
            // Nested classes and structures.
            // =================================

            // ...

            // =================================	
            // Variables.
            // =================================

            // ...

            public float speed = 2.0f;
            public Vector2 maxRotation = new Vector2(8.0f, 8.0f);

            public bool topDownView = false;

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

            void Update()
            {
                Vector2 mousePosition = Input.mousePosition;

                float screenWidthHalf = Screen.width / 2.0f;
                float screenHeightHalf = Screen.height / 2.0f;

                mousePosition.x = (mousePosition.x - screenWidthHalf) / screenWidthHalf;
                mousePosition.y = (mousePosition.y - screenHeightHalf) / screenHeightHalf;

                Vector3 targetRotationEuler = Vector3.zero;

                targetRotationEuler.x = maxRotation.x * mousePosition.x;
                targetRotationEuler.y = maxRotation.y * -mousePosition.y;

                Quaternion targetRotation;

                if (!topDownView)
                {
                    targetRotation =
                        Quaternion.AngleAxis(targetRotationEuler.x, Vector3.up) *       // Left-right.
                        Quaternion.AngleAxis(targetRotationEuler.y, Vector3.right);     // Up-down.
                }
                else
                {
                    targetRotation =
                        Quaternion.AngleAxis(targetRotationEuler.y, Vector3.right) *    // Left-right.
                        Quaternion.AngleAxis(targetRotationEuler.x, Vector3.forward);   // Up-down.
                }

                transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, Time.deltaTime * speed);
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