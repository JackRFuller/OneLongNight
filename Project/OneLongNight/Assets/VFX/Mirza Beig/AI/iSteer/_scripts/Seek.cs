
// =================================	
// Namespaces.
// =================================

using UnityEngine;
using System.Collections;

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

            [RequireComponent(typeof(Agent))]

            public class Seek : MonoBehaviour
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

                Agent agent;

                public SeekTarget target;

                // Seek force scale.

                public float speed = 32.0f;
                
                // Seek while in range.

                public float visionRadius = 32.0f;
                
                // Greater distance from target == less force.

                public bool scaleByDistance = false;

                // ...

                [Header("Debug")]

                public bool drawGizmos = false;

                public Color visionRadiusColour = Color.yellow;

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
                    agent = GetComponent<Agent>();
                }

                // ...

                void FixedUpdate()
                {
                    if (target)
                    {
                        Vector3 force = SteeringForces.seek(

                            transform.position, target.transform.position,
                            speed, target.arrivalRadius, target.arrivedRadius, visionRadius, scaleByDistance);

                        agent.rigidbody.AddForce(force);
                    }
                }

                // ...

                void OnDrawGizmos()
                {
                    if (drawGizmos)
                    {
                        Gizmos.color = visionRadiusColour;
                        Gizmos.DrawWireSphere(transform.position, visionRadius);
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
