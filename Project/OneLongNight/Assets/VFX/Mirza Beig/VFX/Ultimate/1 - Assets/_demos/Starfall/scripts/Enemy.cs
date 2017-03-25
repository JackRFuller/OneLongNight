
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

                //[RequireComponent(typeof(TrailRenderer))]

                public class Enemy : MonoBehaviour
                {
                    // =================================	
                    // Nested classes and structures.
                    // =================================

                    // ...

                    // =================================	
                    // Variables.
                    // =================================

                    // Components.

                    Rigidbody rigidbody;

                    // Stats.

                    [Header("Stats")]

                    public float speed = 10.0f;
                    public float turnSpeed = 8.0f;

                    Quaternion rotation = Quaternion.identity;

                    public Renderer renderer;
                    public float colourVariation = 0.1f;

                    // Weapons.

                    [Header("Weapons")]

                    public Weapon weapon;

                    // AI.

                    [Header("AI")]

                    public float avoidOtherEnemiesForce = 20.0f;
                    public float avoidOtherEnemiesRadius = 3.0f;

                    public float visionRadius = 25.0f;

                    public float targetArrivalRadius = 10.0f;
                    public float targetArrivedRadius = 2.0f;

                    public float originArrivalRadius = 20.0f;
                    public float originArrivedRadius = 10.0f;

                    // VFX.

                    [Header("VFX")]

                    public ParticleSystems ps_spawn;
                    public ParticleSystems ps_destroyed;

                    public ParticleSystems ps_thruster;
                    public ParticleSystems ps_thruster_reverse;

                    public ParticleSystems ps_thruster_left;
                    public ParticleSystems ps_thruster_right;

                    public ParticleSystems ps_thruster_left_reverse;
                    public ParticleSystems ps_thruster_right_reverse;

                    public Shaker.Parameters onDestroyCameraShakeParams;

                    // Input.

                    [Header("Input")]

                    // If velocity (length) is within this range, ship isn't thrusting.

                    public float velocityEpsilon = 2.0f;

                    // If rotation delta is within this range (degrees), rotation is complete.

                    public float rotationDeltaEpsilon = 0.1f;

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

                        // Spawn randomly within this radius from the origin.

                        Vector3 randomPosition = Random.insideUnitSphere * GameManager.instance.enemyRandomSpawnRadius;

                        //randomPosition.y = 0.0f;
                        transform.position = randomPosition;

                        Instantiate(ps_spawn, transform.position, Quaternion.identity);

                        // Randomize colour of the material (slightly).

                        Color colour = renderer.material.color;

                        colour.r += Random.Range(-colourVariation, colourVariation);
                        colour.g += Random.Range(-colourVariation, colourVariation);
                        colour.b += Random.Range(-colourVariation, colourVariation);

                        renderer.material.color = colour;
                    }

                    // ...

                    void Update()
                    {
                        // Face direction of velocity.

                        Quaternion targetRotation = Quaternion.identity;

                        if (rigidbody.velocity != Vector3.zero)
                        {
                            targetRotation = Quaternion.LookRotation(rigidbody.velocity);
                        }

                        // Smooth this rotation a bit.

                        rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);

                        // VFX.

                        // Thruster VFX.
                        // Forward-backward thrusters.

                        Vector3 localVelocity = transform.InverseTransformDirection(rigidbody.velocity);

                        if (localVelocity.magnitude < velocityEpsilon)
                        {
                            localVelocity = Vector3.zero;
                        }

                        if (localVelocity.z > 0.0f)
                        {
                            if (!ps_thruster.isPlaying())
                            {
                                ps_thruster.play();
                            }

                            ps_thruster_reverse.stop();
                        }
                        else if (localVelocity.z < 0.0f)
                        {
                            if (!ps_thruster_reverse.isPlaying())
                            {
                                ps_thruster_reverse.play();
                            }

                            ps_thruster.stop();
                        }
                        else
                        {
                            ps_thruster.stop();
                            ps_thruster_reverse.stop();
                        }

                        // Side thrusters.

                        //float directionOfRotation;
                        //Vector3 directionToPlayer = transform.position - GameManager.instance.player.transform.position;

                        //if (directionToPlayer.x < 0.0f)
                        //{
                        //    directionOfRotation = 1.0f;
                        //}
                        //else
                        //{
                        //    directionOfRotation = -1.0f;
                        //}

                        //if (localVelocity.z >= 0.0f)
                        //{
                        //    if (directionOfRotation > 0.0f)
                        //    {
                        //        if (!ps_thruster_left.isPlaying())
                        //        {
                        //            ps_thruster_left.play();
                        //        }

                        //        ps_thruster_right.stop();
                        //    }
                        //    else if (directionOfRotation < 0.0f)
                        //    {
                        //        if (!ps_thruster_right.isPlaying())
                        //        {
                        //            ps_thruster_right.play();
                        //        }

                        //        ps_thruster_left.stop();
                        //    }
                        //    else
                        //    {
                        //        ps_thruster_left.stop();
                        //        ps_thruster_right.stop();
                        //    }

                        //    ps_thruster_left_reverse.stop();
                        //    ps_thruster_right_reverse.stop();
                        //}
                        //else
                        //{
                        //    if (directionOfRotation < 0.0f)
                        //    {
                        //        if (!ps_thruster_left_reverse.isPlaying())
                        //        {
                        //            ps_thruster_left_reverse.play();
                        //        }

                        //        ps_thruster_right_reverse.stop();
                        //    }
                        //    else if (directionOfRotation > 0.0f)
                        //    {
                        //        if (!ps_thruster_right_reverse.isPlaying())
                        //        {
                        //            ps_thruster_right_reverse.play();
                        //        }

                        //        ps_thruster_left_reverse.stop();
                        //    }
                        //    else
                        //    {
                        //        ps_thruster_left_reverse.stop();
                        //        ps_thruster_right_reverse.stop();
                        //    }

                        //    ps_thruster_left.stop();
                        //    ps_thruster_right.stop();
                        //}
                    }

                    // ...

                    void FixedUpdate()
                    {
                        Vector3 force = Vector3.zero;
                        Vector3 targetPosition = GameManager.instance.player.transform.position;

                        // If player is beyond (vision) range, seek to origin. 

                        if ((targetPosition - transform.position).magnitude > visionRadius)
                        {
                            force += SteeringForces.seek(transform.position,
                                Vector3.zero, speed, originArrivalRadius, originArrivedRadius);
                        }

                        // Else, hunt down player.

                        else
                        {
                            force += SteeringForces.seek(transform.position,
                                targetPosition, speed, targetArrivalRadius, targetArrivedRadius);
                        }

                        Collider[] colliders = UnityEngine.Physics.OverlapSphere(transform.position, avoidOtherEnemiesRadius);

                        for (int i = 0; i < colliders.Length; i++)
                        {
                            Enemy other = colliders[i].GetComponentInParent<Enemy>();

                            if (other)
                            {
                                if (other != this)
                                {
                                    force += -SteeringForces.seek(transform.position, other.transform.position, avoidOtherEnemiesForce, avoidOtherEnemiesRadius);
                                }
                            }
                        }

                        rigidbody.AddForce(force);
                        rigidbody.MoveRotation(rotation);
                    }

                    // ...

                    void OnCollisionEnter(Collision other)
                    {
                        Bullet bullet = other.collider.GetComponentInParent<Bullet>();

                        if (bullet)
                        {
                            GameManager.instance.cameraShake.shake(onDestroyCameraShakeParams);
                            Instantiate(ps_destroyed, transform.position, Quaternion.identity);

                            Destroy(gameObject);
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

}

// =================================	
// --END-- //
// =================================
