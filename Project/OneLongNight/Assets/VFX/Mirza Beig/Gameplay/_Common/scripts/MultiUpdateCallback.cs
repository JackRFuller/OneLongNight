
// =================================	
// Namespaces.
// =================================

using UnityEngine;
using UnityEngine.Events;

//using System.Collections;

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

        //[RequireComponent(typeof(TrailRenderer))]

        public class MultiUpdateCallback : MonoBehaviour
        {
            // =================================	
            // Nested classes and structures.
            // =================================

            // ...

            // =================================	
            // Variables.
            // =================================

            // ...

            public UpdateCallback updateCallback = UpdateCallback.fixedUpdate;
            public UnityEvent unityEvent;

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

            void update()
            {
                unityEvent.Invoke();
            }

            // ...

            void Update()
            {
                if (updateCallback == UpdateCallback.update)
                {
                    update();
                }
            }

            // ...

            void FixedUpdate()
            {
                if (updateCallback == UpdateCallback.fixedUpdate)
                {
                    update();
                }
            }

            // ...

            void LateUpdate()
            {
                if (updateCallback == UpdateCallback.lateUpdate)
                {
                    update();
                }
            }

            // ...

            public void reset()
            {

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
