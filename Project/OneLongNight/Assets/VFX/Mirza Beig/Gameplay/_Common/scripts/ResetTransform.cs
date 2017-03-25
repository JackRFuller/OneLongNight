
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

        public class ResetTransform : MonoBehaviour
        {
            // =================================	
            // Nested classes and structures.
            // =================================

            // ...

            // =================================	
            // Variables.
            // =================================

            // ...

            Vector3 startPosition;
            Quaternion startRotation;

            public KeyCode resetOnKey = KeyCode.R;

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
                startPosition = transform.localPosition;
                startRotation = transform.localRotation;
            }

            // ...

            void Update()
            {
                if (Input.GetKeyDown(resetOnKey))
                {
                    reset();
                }
            }

            // ...

            public void reset()
            {
                transform.localPosition = startPosition;
                transform.localRotation = startRotation;

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
