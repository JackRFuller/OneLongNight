
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

        public class MouseRotate : MonoBehaviour
        {
            // =================================	
            // Nested classes and structures.
            // =================================

            // ...

            // =================================	
            // Variables.
            // =================================

            // ...

            public float speed = 8.0f;
            public float lerpSpeed = Mathf.Infinity;

            public UpdateCallback updateCallback = UpdateCallback.fixedUpdate;

            Vector3 targetRotation;

            public bool clickAndDragToMove = true;

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
                bool updateMouse = Input.GetMouseButtonDown(0) || !clickAndDragToMove;

                if (updateMouse)
                {
                    //Cursor.lockState = CursorLockMode.Locked;
                    targetRotation = transform.localEulerAngles;
                }
                if (Input.GetMouseButtonUp(0))
                {
                    //Cursor.lockState = CursorLockMode.None;
                }

                if (updateMouse)
                {
                    targetRotation.x -= Input.GetAxis("Mouse Y") * speed;
                    targetRotation.y += Input.GetAxis("Mouse X") * speed;
                }

                transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(targetRotation), Time.deltaTime * lerpSpeed);
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
