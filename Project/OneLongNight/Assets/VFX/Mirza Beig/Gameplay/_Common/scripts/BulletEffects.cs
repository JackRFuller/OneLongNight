
// =================================	
// Namespaces.
// =================================

using UnityEngine;

using System.Collections;
using System.Collections.Generic;

using MirzaBeig.VFX;

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

        [RequireComponent(typeof(Bullet))]

        public class BulletEffects : MonoBehaviour
        {
            // =================================	
            // Nested classes and structures.
            // =================================

            // ...

            // =================================	
            // Variables.
            // =================================

            // Bullet component needs to be on this object.

            public Bullet bullet { get; private set; }

            // Particles emitted when destroyed on impact.
            // Particles emitted when destroyed after max life.

            public GameObject pf_vfx_hitDestroy;
            public GameObject pf_vfx_timeoutDestroy;

            // Unparent these specified objects on destroy.
            // Useful for trails, particles, etc.

            public List<Transform> objectsToUnparentOnDestroy;
            public Transform objectsToUnparentTarget;

            // Optionally, choose to parent those objects to bullet's parent.
            // This overrides the assigned target.

            public bool unparentSelectObjectsToBulletParent = false;

            // Register bullet events?
            // Else, you'll have to call the functions manually.

            // Useful for networking events from another class.

            public bool registerEvents = true;

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
                if (registerEvents)
                {
                    bullet.onMaxLifeEvent += onMaxLife;

                    bullet.onTriggerEnterEvent += onTriggerEnter;
                    bullet.onCollisionEnterEvent += onCollisionEnter;

                    bullet.onPreDestroyEvent += onPreDestroy;
                }
            }

            // ...

            void Update()
            {

            }

            // ...

            void onMaxLife()
            {
                instantiateTimeoutDestroyVFX();
            }

            // ...

            void onTriggerEnter(Bullet bullet, Collider collider)
            {
                instantiateHitDestroyVFX();
            }

            // ...

            void onCollisionEnter(Bullet bullet, Collision collision)
            {
                instantiateHitDestroyVFX();
            }

            // ...

            protected void instantiateParticles(GameObject gameObject)
            {
                GameObject obj = Instantiate(gameObject);
                obj.transform.position = transform.position;

                BulletParticles bulletParticles =
                    gameObject.GetComponent<BulletParticles>();

                if (bulletParticles)
                {
                    if (bulletParticles.orientToBullet)
                    {
                        obj.transform.rotation = transform.rotation;
                    }
                }

                //objectsToUnparentOnDestroy.Add(obj.transform);
            }

            // ...

            public void instantiateHitDestroyVFX()
            {
                instantiateParticles(pf_vfx_hitDestroy);
            }

            public void instantiateTimeoutDestroyVFX()
            {
                instantiateParticles(pf_vfx_timeoutDestroy);
            }

            // For some reason, particles are cleared when calling Destroy()
            // even though trails and the like are fine as is. I've setup
            // this special event that should be triggered before any Destroy()
            // call is made.

            // Also, NEVER instantiate in OnDestroy() directly.
            // Since you'll get objects that persist in the editor.}

            void onPreDestroy()
            {
                unparentObjects();
            }

            public void unparentObjects()
            {
                Transform targetParent =
                    unparentSelectObjectsToBulletParent ? transform.parent : objectsToUnparentTarget;

                for (int i = 0; i < objectsToUnparentOnDestroy.Count; i++)
                {
                    // In the case of DestroyOnParticlesDead, the object may not exist any more.

                    if (objectsToUnparentOnDestroy[i])
                    {
                        ParticleSystems particleSystems =
                            objectsToUnparentOnDestroy[i].GetComponent<ParticleSystems>();

                        if (particleSystems)
                        {
                            particleSystems.stop();
                            particleSystems.setLoop(false);
                        }

                        objectsToUnparentOnDestroy[i].parent = targetParent;
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
