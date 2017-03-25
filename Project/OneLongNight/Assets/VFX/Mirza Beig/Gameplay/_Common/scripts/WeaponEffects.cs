
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

    namespace Gameplay
    {

        // =================================	
        // Classes.
        // =================================

        //[ExecuteInEditMode]
        [System.Serializable]

        [RequireComponent(typeof(Weapon))]

        public class WeaponEffects : MonoBehaviour
        {
            // =================================	
            // Nested classes and structures.
            // =================================

            // ...

            // =================================	
            // Variables.
            // =================================

            // ...

            Weapon weapon;

            // Particles emitted when firing.

            public GameObject pf_particles_shoot;

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
                weapon = GetComponent<Weapon>();
                weapon.onBulletInstantiatedEvent += onInstantiateBullet;
            }

            // ...

            void onInstantiateBullet(Bullet bullet)
            {
                instantiateShootParticles();
            }

            // ...

            void instantiateShootParticles()
            {
                GameObject obj = Instantiate(pf_particles_shoot, 
                    weapon.bulletSpawnPoint.position, weapon.bulletSpawnPoint.rotation) as GameObject;

                obj.transform.parent = transform;
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
