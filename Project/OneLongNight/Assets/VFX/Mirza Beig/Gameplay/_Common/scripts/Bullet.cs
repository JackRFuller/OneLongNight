
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

        //[RequireComponent(typeof(Collider))]
        [RequireComponent(typeof(Rigidbody))]

        public class Bullet : MonoBehaviour
        {
            // =================================	
            // Nested classes and structures.
            // =================================

            // ...

            // =================================	
            // Variables.
            // =================================

            // ...

            public Rigidbody rigidbody { get; private set; }
            public Collider[] colliders { get; private set; }

            // ...

            float maxLifeTimer = 0.0f;
            public float maxLife = 2.0f;

            public int damage = 1;

            bool setToDestroy = false;

            // Destroy on timeout or impact.
            // Else, will not be destroyed on its own.

            // Useful for manually handling destruction with networking (NetworkServer.Destroy).
            // Set to FALSE when networking and you want to use NetworkServer.Destroy.

            public bool autoDestruct = true;

            // Event delegates.
            
            public delegate void onTriggerEnterEventHandler(Bullet bullet, Collider collider);
            public event onTriggerEnterEventHandler onTriggerEnterEvent;

            public delegate void onCollisionEnterEventHandler(Bullet bullet, Collision collision);
            public event onCollisionEnterEventHandler onCollisionEnterEvent;

            public delegate void onDestroyEventHandler(Bullet bullet);
            public event onDestroyEventHandler onDestroyEvent;

            public delegate void onMaxLifeEventHandler();
            public event onMaxLifeEventHandler onMaxLifeEvent;

            public delegate void onPreDestroyEventHandler();

            public event onPreDestroyEventHandler onPreDestroyEvent;            
            public event onPreDestroyEventHandler onPreDestroyEvent2;

            // =================================	
            // Functions.
            // =================================

            // ...

            protected virtual void Awake()
            {
                rigidbody = GetComponent<Rigidbody>();
                colliders = GetComponentsInChildren<Collider>();
            }

            // ...

            protected virtual void Start()
            {

            }

            // ...

            protected virtual void Update()
            {
                if (!setToDestroy)
                {
                    maxLifeTimer += Time.deltaTime;

                    if (maxLifeTimer >= maxLife)
                    {
                        setToDestroy = true;

                        if (onMaxLifeEvent != null)
                        {
                            onMaxLifeEvent();
                        }

                        if (onPreDestroyEvent != null)
                        {
                            onPreDestroyEvent();
                        }
                        if (onPreDestroyEvent2 != null)
                        {
                            onPreDestroyEvent2();
                        }

                        destroy();
                    }
                }
            }

            // ...

            protected void sendOnPreDestroyEvent()
            {
                if (onPreDestroyEvent != null)
                {
                    onPreDestroyEvent();
                }
            }

            // ...

            protected virtual void OnTriggerEnter(Collider collider)
            {
                if (onTriggerEnterEvent != null)
                {
                    onTriggerEnterEvent(this, collider);
                }

                if (onPreDestroyEvent != null)
                {
                    onPreDestroyEvent();
                }
                if (onPreDestroyEvent2 != null)
                {
                    onPreDestroyEvent2();
                }

                destroy();
            }

            // ...

            protected virtual void OnCollisionEnter(Collision collision)
            {
                if (onCollisionEnterEvent != null)
                {
                    onCollisionEnterEvent(this, collision);
                }

                if (onPreDestroyEvent != null)
                {
                    onPreDestroyEvent();
                }
                if (onPreDestroyEvent2 != null)
                {
                    onPreDestroyEvent2();
                }

                destroy();
            }

            // ...

            void destroy()
            {
                if (autoDestruct)
                {
                    Destroy(gameObject);
                }
            }

            // ...

            protected virtual void OnDestroy()
            {
                if (onDestroyEvent != null)
                {
                    onDestroyEvent(this);
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
