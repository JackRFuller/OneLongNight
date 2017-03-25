
// =================================	
// Namespaces.
// =================================

using UnityEngine;
//using System.Collections;

using MirzaBeig.Common;
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

                public class Player : MonoBehaviour
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
                    public float tiltSpeed = 8.0f;
                    public float rollSpeed = 8.0f;

                    public Vector2 deltaRotationYToRollZRange = new Vector2(15.0f, 25.0f);

                    public float seekToOriginArrivalRadius = 10.0f;
                    public float seekToOriginArrivedRadius = 4.0f;

                    public float moveToZeroSpeedY = 2.0f;

                    // Weapons.

                    [Header("Weapons")]

                    public Weapon weapon;
                    public float weaponRecoil = 25.0f;

                    // UI.

                    [Header("UI")]

                    public MouseFollowUI reticle;

                    // VFX.

                    [Header("VFX")]

                    public ParticleSystems blackHole;

                    public Shaker.Parameters cameraShakeParams;

                    public ParticleSystems ps_thruster;
                    public ParticleSystems ps_thruster_reverse;

                    public ParticleSystems ps_thruster_left;
                    public ParticleSystems ps_thruster_right;

                    public ParticleSystems ps_thruster_left_reverse;
                    public ParticleSystems ps_thruster_right_reverse;

                    // Input.

                    Vector2 input;
                    Vector3 reticleScreenPosition;

                    // Angle deltas.

                    float rotationEulerX;
                    float rotationEulerY;
                    float rotationEulerZ;

                    float lastRotationEulerY;

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
                    }

                    // ...

                    void Update()
                    {
                        // Input.

                        input.x = Input.GetAxis("Horizontal");
                        input.y = Input.GetAxis("Vertical");

                        if (Input.GetMouseButton(1))
                        {
                            input.y += 1.0f;
                        }

                        // Rotate.

                        Vector3 localVelocity = transform.InverseTransformDirection(rigidbody.velocity);

                        if (localVelocity.magnitude < velocityEpsilon)
                        {
                            localVelocity = Vector3.zero;
                        }

                        Quaternion lookRotation = Quaternion.identity;

                        if (localVelocity != Vector3.zero)
                        {
                            lookRotation = Quaternion.LookRotation(localVelocity);
                        }

                        rotationEulerX = Mathf.LerpAngle(rotationEulerX, lookRotation.eulerAngles.x, Time.deltaTime * turnSpeed);

                        reticleScreenPosition = RectTransformUtility.WorldToScreenPoint(reticle.canvas.worldCamera, reticle.transform.position);
                        Vector3 myScreenPosition = Camera.main.WorldToScreenPoint(transform.position);

                        Vector3 direction = reticleScreenPosition - myScreenPosition;

                        float angleToMouseTemp = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                        float angleToMouse = -angleToMouseTemp + 90.0f;

                        rotationEulerY = Mathf.LerpAngle(rotationEulerY, angleToMouse, Time.deltaTime * turnSpeed);

                        // Weapons.

                        if (Input.GetKey(KeyCode.E))
                        {
                            if (Input.GetMouseButton(0))
                            {
                                Bullet bullet = weapon.shoot(rigidbody.velocity);

                                if (bullet)
                                {
                                    rigidbody.AddForceAtPosition(-weapon.transform.forward * weaponRecoil, weapon.transform.position);
                                    GameManager.instance.cameraShake.shake(cameraShakeParams);
                                }
                            }
                        }
                        else if (Input.GetMouseButtonDown(0))
                        {
                            Bullet bullet = weapon.shoot(rigidbody.velocity, true);

                            if (bullet)
                            {
                                rigidbody.AddForceAtPosition(-weapon.transform.forward * weaponRecoil, weapon.transform.position);
                                GameManager.instance.cameraShake.shake(cameraShakeParams);
                            }
                        }

                        // VFX.

                        if (Input.GetKeyDown(KeyCode.Space))
                        {
                            blackHole.play();
                        }
                        if (Input.GetKeyUp(KeyCode.Space))
                        {
                            blackHole.stop();
                        }

                        // Thruster VFX.
                        // Forward-backward thrusters.

                        if (input.y > 0.0f)
                        {
                            if (!ps_thruster.isPlaying())
                            {
                                ps_thruster.play();
                            }

                            ps_thruster_reverse.stop();
                        }
                        else if (input.y < 0.0f)
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

                        float rotationDelta = rotationEulerY - lastRotationEulerY;
                        float rotationDeltaRounded;

                        if (Mathf.Abs(rotationDelta) < rotationDeltaEpsilon)
                        {
                            rotationDeltaRounded = 0.0f;
                        }
                        else
                        {
                            rotationDeltaRounded = rotationDelta;
                        }

                        float rotationEulerTargetZ = MathUtility.remap(-rotationDeltaRounded,

                            -deltaRotationYToRollZRange.x, deltaRotationYToRollZRange.x,
                            -deltaRotationYToRollZRange.y, deltaRotationYToRollZRange.y);

                        //rotationEulerZ = -rotationEulerTargetZ;
                        rotationEulerZ = Mathf.LerpAngle(rotationEulerZ, rotationEulerTargetZ, Time.deltaTime * rollSpeed);

                        if (input.y >= 0.0f)
                        {
                            if (rotationDeltaRounded > 0.0f)
                            {
                                if (!ps_thruster_left.isPlaying())
                                {
                                    ps_thruster_left.play();
                                }

                                ps_thruster_right.stop();
                            }
                            else if (rotationDeltaRounded < 0.0f)
                            {
                                if (!ps_thruster_right.isPlaying())
                                {
                                    ps_thruster_right.play();
                                }

                                ps_thruster_left.stop();
                            }
                            else
                            {
                                ps_thruster_left.stop();
                                ps_thruster_right.stop();
                            }

                            ps_thruster_left_reverse.stop();
                            ps_thruster_right_reverse.stop();
                        }
                        else
                        {
                            if (rotationDeltaRounded < 0.0f)
                            {
                                if (!ps_thruster_left_reverse.isPlaying())
                                {
                                    ps_thruster_left_reverse.play();
                                }

                                ps_thruster_right_reverse.stop();
                            }
                            else if (rotationDeltaRounded > 0.0f)
                            {
                                if (!ps_thruster_right_reverse.isPlaying())
                                {
                                    ps_thruster_right_reverse.play();
                                }

                                ps_thruster_left_reverse.stop();
                            }
                            else
                            {
                                ps_thruster_left_reverse.stop();
                                ps_thruster_right_reverse.stop();
                            }

                            ps_thruster_left.stop();
                            ps_thruster_right.stop();
                        }
                    }

                    // ...

                    void FixedUpdate()
                    {
                        // Rotate.

                        Quaternion rotationX = Quaternion.AngleAxis(rotationEulerX, Vector3.right);
                        Quaternion rotationY = Quaternion.AngleAxis(rotationEulerY, Vector3.up);
                        Quaternion rotationZ = Quaternion.AngleAxis(rotationEulerZ, Vector3.forward);

                        Quaternion rotation = rotationX * rotationY * rotationZ;

                        rigidbody.MoveRotation(rotation);

                        // Move.

                        Vector3 force = Vector3.zero;
                        Vector3 forwardXZ = rotationY * Vector3.forward;

                        force += forwardXZ * input.y;

                        // Always seek to origin on Y-axis (seek y = 0.0f).

                        Vector3 seekToOriginY = SteeringForces.seek(transform.position, Vector3.zero, 1.0f, seekToOriginArrivalRadius, seekToOriginArrivedRadius);

                        seekToOriginY.x = 0.0f;
                        seekToOriginY.z = 0.0f;

                        force += seekToOriginY;

                        force *= speed;

                        rigidbody.AddForce(force);

                        // Lock position on Y. Attempts to smoothly move player back to 0.0f.

                        //Vector3 position = transform.position;
                        //position.y = Mathf.Lerp(position.y, 0.0f, Time.deltaTime * moveToZeroSpeedY);

                        //rigidbody.MovePosition(position);
                    }

                    // ...

                    void LateUpdate()
                    {
                        lastRotationEulerY = rotationEulerY;
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