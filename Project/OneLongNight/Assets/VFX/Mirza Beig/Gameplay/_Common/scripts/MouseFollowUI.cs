
// =================================	
// Namespaces.
// =================================

using UnityEngine;
using System.Collections;

using UnityEngine.UI;

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

        public class MouseFollowUI : MonoBehaviour
        {
            // =================================	
            // Nested classes and structures.
            // =================================

            // ...

            // =================================	
            // Variables.
            // =================================

            public float speed = Mathf.Infinity;

            public Canvas canvas { get; set; }
            RectTransform canvasTransform;

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
                canvas = GetComponentInParent<Canvas>();
                canvasTransform = canvas.transform as RectTransform;
            }

            // ...

            void Update()
            {
                Vector2 mousePositionUI;

                RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasTransform,
                     Input.mousePosition, canvas.worldCamera, out mousePositionUI);

                Vector3 targetPosition = canvas.transform.TransformPoint(mousePositionUI);
                transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * speed);
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
