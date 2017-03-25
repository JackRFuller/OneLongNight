
// =================================	
// Namespaces.
// =================================

using UnityEngine;
//using System.Collections;

using MirzaBeig.Gameplay;
using MirzaBeig.AI.iSteer;

// =================================	
// Define namespace.
// =================================

namespace MirzaBeig
{

    namespace VFX
    {

        namespace Ultimate
        {

            namespace Starfall
            {

                // =================================	
                // Classes.
                // =================================

                //[ExecuteInEditMode]
                [System.Serializable]

                [RequireComponent(typeof(Bullet))]

                public class HomingBullet : MonoBehaviour
                {
                    // =================================	
                    // Nested classes and structures.
                    // =================================

                    // ...

                    // =================================	
                    // Variables.
                    // =================================

                    // ...

                    Bullet bullet;

                    // ...

                    public float speed = 10.0f;
                    public float checkForEnemiesRadius = 10.0f;

                    // =================================	
                    // Functions.
                    // =================================

                    // ...

                    void Awake()
                    {
                        bullet = GetComponent<Bullet>();
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

                    void FixedUpdate()
                    {
                        Vector3 force = Vector3.zero;
                        Collider[] colliders = UnityEngine.Physics.OverlapSphere(transform.position, checkForEnemiesRadius);

                        for (int i = 0; i < colliders.Length; i++)
                        {
                            Enemy enemy = colliders[i].GetComponentInParent<Enemy>();

                            if (enemy)
                            {
                                force += SteeringForces.seek(transform.position, enemy.transform.position, speed);

                                break;
                            }
                        }

                        bullet.rigidbody.AddForce(force);
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

}

// =================================	
// --END-- //
// =================================