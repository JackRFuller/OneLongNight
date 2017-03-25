
// =================================	
// Namespaces.
// =================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;

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

        public class BulletParticles : DestroyOnParticlesDead
        {
            // =================================	
            // Nested classes and structures.
            // =================================

            // ...

            // =================================	
            // Variables.
            // =================================

            // ...

            public bool orientToBullet = false;

            // =================================	
            // Functions.
            // =================================

            // ...

            protected override void Awake()
            {
                base.Awake();
            }

            // ...

            protected override void Start()
            {
                base.Start();
            }

            // ...

            protected override void Update()
            {
                base.Update();
            }

            // ...

            protected override void LateUpdate()
            {
                base.LateUpdate();
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
