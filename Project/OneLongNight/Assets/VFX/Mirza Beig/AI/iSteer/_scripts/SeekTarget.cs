
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

            //[RequireComponent(typeof(Rigidbody))]

            public class SeekTarget : MonoBehaviour
            {
                // =================================	
                // Nested classes and structures.
                // =================================

                // ...

                // =================================	
                // Variables.
                // =================================

                // ...

                [Header("Settings")]

                public float arrivalRadius = 8.0f; // Slow down.
                public float arrivedRadius = 4.0f; // Stop seeking.

                // ...

                [Header("Debug")]

                public bool drawGizmos = false;

                public Color arrivalRadiusColour = Color.red;
                public Color arrivedRadiusColour = Color.gray;

                //public Color velocityColour = Color.green;

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

                void OnDrawGizmos()
                {
                    if (drawGizmos)
                    {
                        Gizmos.color = arrivalRadiusColour;
                        Gizmos.DrawWireSphere(transform.position, arrivalRadius);

                        Gizmos.color = arrivedRadiusColour;
                        Gizmos.DrawWireSphere(transform.position, arrivedRadius);
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
