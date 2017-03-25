
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

        public class ExplodeOnDestroy : MonoBehaviour
        {
            // =================================	
            // Nested classes and structures.
            // =================================

            // ...

            // =================================	
            // Variables.
            // =================================

            // ...

            public float radius = 5.0f;
            public float force = 10.0f;

            public float upwardsModifier = 4.0f;

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

            }

            // ...

            void Update()
            {

            }
            
            // ...

            void OnDestroy()
            {
                Vector3 position = transform.position;
                Collider[] colliders = UnityEngine.Physics.OverlapSphere(position, radius);

                for (int i = 0; i < colliders.Length; i++)
                {
                    Rigidbody rigidbody = colliders[i].GetComponentInParent<Rigidbody>();

                    if (rigidbody)
                    {
                        rigidbody.AddExplosionForce(force, position, radius, upwardsModifier, forceMode);
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
