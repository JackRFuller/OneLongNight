
// =================================	
// Namespaces.
// =================================

using UnityEngine;
//using System.Collections;

using MirzaBeig.Gameplay;
using MirzaBeig.Animation;

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

                public class GameManager : MonoBehaviour
                {
                    // =================================	
                    // Nested classes and structures.
                    // =================================

                    // ...

                    // =================================	
                    // Variables.
                    // =================================

                    // ...

                    public static GameManager instance;

                    // ...

                    public Player player;
                    public Enemy pf_enemy;

                    public Timer spawnTimer;

                    public float enemyRandomSpawnRadius = 50.0f;

                    public Shaker cameraShake;

                    // =================================	
                    // Functions.
                    // =================================

                    // ...

                    void Awake()
                    {
                        instance = this;
                    }

                    // ...

                    void Start()
                    {
                        onSpawnTimerComplete();
                        spawnTimer.onTimerCompleteEvent += onSpawnTimerComplete;
                    }

                    // ...

                    void Update()
                    {
                        spawnTimer.update();

                        if (Input.GetKeyDown(KeyCode.G))
                        {
                            spawnEnemy();
                        }
                    }

                    // ...

                    void spawnEnemy()
                    {
                        Instantiate(pf_enemy);
                    }

                    // ...

                    void onSpawnTimerComplete()
                    {
                        spawnEnemy();
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
