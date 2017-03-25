
// =================================	
// Namespaces.
// =================================

using UnityEngine;

using System.Collections;
using System.Collections.Generic;

// =================================	
// Define namespace.
// =================================

namespace MirzaBeig
{

    namespace AI
    {

        namespace iSteer
        {

            // =================================	
            // Classes.
            // =================================

            //[ExecuteInEditMode]
            [System.Serializable]

            [RequireComponent(typeof(Rigidbody))]

            public class Agent : MonoBehaviour
            {
                // =================================	
                // Nested classes and structures.
                // =================================

                // ...

                // =================================	
                // Variables.
                // =================================

                // ...

                public Rigidbody rigidbody { get; set; }

                // ...

                [Header("Debug")]

                public bool drawGizmos = false;
                public Color velocityColour = Color.green;

                // =================================	
                // Functions.
                // =================================

                // ...

                void Awake()
                {
                    rigidbody = GetComponent<Rigidbody>();
                }

                // ...

                void Start()
                {

                }
                
                // ...

                void OnDrawGizmos()
                {
                    if (drawGizmos)
                    {
                        if (rigidbody)
                        {
                            Gizmos.color = velocityColour;
                            Gizmos.DrawRay(transform.position, rigidbody.velocity);
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

}

// =================================	
// --END-- //
// =================================
