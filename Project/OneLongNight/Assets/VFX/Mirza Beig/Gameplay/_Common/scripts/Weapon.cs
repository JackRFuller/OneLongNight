
// =================================	
// Namespaces.
// =================================

using UnityEngine;

using System.Collections;
using System.Collections.Generic;

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

        //[RequireComponent(typeof(Rigidbody))]

        public class Weapon : MonoBehaviour
        {
            // =================================	
            // Nested classes and structures.
            // =================================

            // ...

            // =================================	
            // Variables.
            // =================================

            // ...

            public Bullet bulletPrefab;
            public Transform bulletSpawnPoint;

            public float bulletSpeed = 25.0f;
            public float maxBulletsPerSecond = 8.0f;

            float cooldownTimer = 1.0f;

            // Keep track of all bullets fired.

            public List<Bullet> bullets { get; private set; }

            // Link bullets to this tranform on instantiation.
            // Mostly just for organization. Leave it empty to
            // spawn in scene root.

            public Transform bulletContainer;

            // Make bullet's colliders ignore collisions with these colliders (Physics.IgnoreCollision).

            public Collider[] bulletIgnoreColliders;

            // Ignore collision with other bullets from this weapon.

            public bool ignoreOtherBulletsFromThisWeapon = true;

            // Event delegates.

            public delegate void onBulletInstantiatedEventHandler(Bullet bullet);
            public event onBulletInstantiatedEventHandler onBulletInstantiatedEvent;

            // =================================	
            // Functions.
            // =================================

            // ...

            protected virtual void Awake()
            {

            }

            // ...

            protected virtual void Start()
            {
                bullets = new List<Bullet>();
            }

            // relativeVelocity takes into account moving weapons.
            // So bullet accelerates with the momentum of the shooter.

            // If force, shoot even if cooldown isn't complete.

            Bullet _shoot(Vector3 direction, Vector3 relativeVelocity, bool ignoreCooldown = false)
            {
                if (cooldownTimer >= 1.0f / maxBulletsPerSecond || ignoreCooldown)
                {
                    cooldownTimer = 0.0f;

                    bulletPrefab.transform.position = bulletSpawnPoint.position;

                    Bullet bullet = Instantiate(bulletPrefab);

                    bullet.transform.parent = bulletContainer;
                    bullet.transform.position = bulletSpawnPoint.position;

                    // Face direction I'm shooting at.

                    bullet.transform.rotation = Quaternion.LookRotation(direction);

                    // But roll with the weapon.

                    bullet.transform.Rotate(-Vector3.forward, MathUtility.getRoll(bulletSpawnPoint.rotation));

                    bullet.onDestroyEvent += onBulletDestroyEvent;
                    bullet.onCollisionEnterEvent += onBulletCollisionEnterEvent;

                    bullet.rigidbody.AddForce(direction * bulletSpeed, ForceMode.Impulse);
                    bullet.rigidbody.AddForce(relativeVelocity, ForceMode.VelocityChange);

                    // Setup collision ignorance.

                    for (int i = 0; i < bullet.colliders.Length; i++)
                    {
                        for (int j = 0; j < bulletIgnoreColliders.Length; j++)
                        {
                            UnityEngine.Physics.IgnoreCollision(bullet.colliders[i], bulletIgnoreColliders[j]);
                        }
                    }

                    // Ignore other bullets from this weapon.

                    for (int i = 0; i < bullet.colliders.Length; i++)
                    {
                        for (int j = 0; j < bullets.Count; j++)
                        {
                            for (int k = 0; k < bullets[j].colliders.Length; k++)
                            {
                                UnityEngine.Physics.IgnoreCollision(bullet.colliders[i], bullets[j].colliders[k]);
                            }
                        }
                    }

                    // Add to list of active bullets.

                    bullets.Add(bullet);

                    if (onBulletInstantiatedEvent != null)
                    {
                        onBulletInstantiatedEvent(bullet);
                    }

                    return bullet;
                }

                return null;
            }

            // Useful for registering a bullet with networking
            // where motion is entirely handled by the server.

            public void initBullet(Bullet bullet)
            {
                // Parent.

                bullet.transform.parent = bulletContainer;

                // Setup events.

                bullet.onDestroyEvent += onBulletDestroyEvent;
                bullet.onCollisionEnterEvent += onBulletCollisionEnterEvent;

                // Setup collision ignorance.

                for (int i = 0; i < bullet.colliders.Length; i++)
                {
                    for (int j = 0; j < bulletIgnoreColliders.Length; j++)
                    {
                        UnityEngine.Physics.IgnoreCollision(bullet.colliders[i], bulletIgnoreColliders[j]);
                    }
                }

                // Ignore other bullets from this weapon.

                for (int i = 0; i < bullet.colliders.Length; i++)
                {
                    for (int j = 0; j < bullets.Count; j++)
                    {
                        for (int k = 0; k < bullets[j].colliders.Length; k++)
                        {
                            UnityEngine.Physics.IgnoreCollision(bullet.colliders[i], bullets[j].colliders[k]);
                        }
                    }
                }

                // Add to list of active bullets.

                bullets.Add(bullet);

                // Call on bullet instantiated event.

                if (onBulletInstantiatedEvent != null)
                {
                    onBulletInstantiatedEvent(bullet);
                }
            }

            // ...

            public virtual Bullet shoot(bool ignoreCooldown = false)
            {
                return _shoot(bulletSpawnPoint.forward, Vector3.zero, ignoreCooldown);
            }

            // ...

            public virtual Bullet shoot(Vector3 relativeVelocity, bool ignoreCooldown = false)
            {
                return _shoot(bulletSpawnPoint.forward, relativeVelocity, ignoreCooldown);
            }

            // ...

            public virtual Bullet shoot(Vector3 direction, Vector3 relativeVelocity, bool ignoreCooldown = false)
            {
                return _shoot(direction, relativeVelocity, ignoreCooldown);
            }

            // ...

            protected virtual void onBulletDestroyEvent(Bullet bullet)
            {
                bullets.Remove(bullet);
            }

            // ...

            protected virtual void onBulletCollisionEnterEvent(Bullet bullet, Collision collision)
            {

            }

            // ...

            protected virtual void Update()
            {
                cooldownTimer += Time.deltaTime;
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
