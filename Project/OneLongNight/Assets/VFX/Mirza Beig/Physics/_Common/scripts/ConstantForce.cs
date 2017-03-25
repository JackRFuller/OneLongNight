
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

    namespace Physics
    {

        // =================================	
        // Classes.
        // =================================

        //[ExecuteInEditMode]
        [System.Serializable]

        [RequireComponent(typeof(Rigidbody))]

        public class ConstantForce : MonoBehaviour
        {
            // =================================	
            // Nested classes and structures.
            // =================================

            // ...

            // =================================	
            // Variables.
            // =================================

            // ...

            Rigidbody rigidbody;

            public Vector3 force = Vector3.forward;
            public float scale = 1.0f;

            public Space relativeTo = Space.Self;
            public ForceMode forceMode = ForceMode.Force;

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
                rigidbody = GetComponent<Rigidbody>();
            }

            // ...

            void Update()
            {

            }

            // ...

            void FixedUpdate()
            {
                Vector3 scaledForce = force * scale;
                
                if (relativeTo == Space.World)
                {
                    rigidbody.AddForce(scaledForce, forceMode);
                }
                else
                {
                    rigidbody.AddRelativeForce(scaledForce, forceMode);
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
