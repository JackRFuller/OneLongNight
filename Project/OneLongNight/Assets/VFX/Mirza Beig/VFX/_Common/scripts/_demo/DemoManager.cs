
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

    namespace VFX
    {

        namespace Demo
        {

            // =================================	
            // Classes.
            // =================================

            //[ExecuteInEditMode]
            [System.Serializable]

            public class DemoManager : MonoBehaviour
            {
                // =================================	
                // Nested classes and structures.
                // =================================

                public enum CameraMode
                {
                    frontFacing,
                    interactive,
                }
                public enum ParticleMode
                {
                    looping,
                    oneshot,
                }

                public enum Level
                {
                    none,
                    basic,
                    forest,
                }

                // =================================	
                // Variables.
                // =================================

                public Vector3 cameraPosition;
                public Vector3 cameraPosition2;

                Vector3 targetCameraPosition;
                Vector3 targetCameraContainerRotation;

                // Because Euler angles wrap around 360, I use
                // a separate value to store the full rotation.

                Vector3 cameraContainerRotation;

                public float cameraSpeed = 2.0f;

                public float cameraLerpTime = 0.2f;
                public float cameraContainerLerpTime = 0.2f;

                public Vector2 cameraAngleLimits = new Vector2(-5.0f, 65.0f);

                public GameObject[] levels;
                public Level currentLevel = Level.basic;

                public CameraMode cameraMode = CameraMode.frontFacing;
                public ParticleMode particleMode = ParticleMode.looping;

                public bool lighting = true;
                public bool advancedRendering = true;

                public Toggle frontFacingCameraModeToggle;
                public Toggle interactiveCameraModeToggle;

                public Toggle loopingParticleModeToggle;
                public Toggle oneshotParticleModeToggle;

                public Toggle lightingToggle;
                public Toggle advancedRenderingToggle;

                Toggle[] levelToggles;
                public ToggleGroup levelTogglesContainer;

                public LoopingParticleSystemsManager loopingParticleSystems;
                public OneshotParticleSystemsManager oneshotParticleSystems;

                public Text particleCountText;
                public Text currentParticleSystemText;

                public Text particleSpawnInstructionText;

                public Slider timeScaleSlider;
                public Text timeScaleSliderValueText;

                public Camera UICamera;
                public Camera mainCamera;
                public Camera postEffectsCamera;

                public MonoBehaviour[] mainCameraPostEffects;

                // =================================	
                // Functions.
                // =================================

                // ...
                
                void Awake()
                {
                    loopingParticleSystems.init();
                    oneshotParticleSystems.init();
                }

                // ...

                void Start()
                {
                    // ...

                    switch (cameraMode)
                    {
                        case CameraMode.frontFacing:
                            {
                                setToFrontFacingCameraMode(true);
                                targetCameraPosition = cameraPosition;

                                frontFacingCameraModeToggle.isOn = true;
                                interactiveCameraModeToggle.isOn = false;

                                break;
                            }
                        case CameraMode.interactive:
                            {
                                setToInteractiveCameraMode(true);
                                targetCameraPosition = cameraPosition2;

                                frontFacingCameraModeToggle.isOn = false;
                                interactiveCameraModeToggle.isOn = true;

                                break;
                            }
                        default:
                            {
                                print("Unknown case.");

                                break;
                            }
                    }

                    // ...

                    switch (particleMode)
                    {
                        case ParticleMode.looping:
                            {
                                setToPerpetualParticleMode(true);

                                loopingParticleModeToggle.isOn = true;
                                oneshotParticleModeToggle.isOn = false;

                                break;
                            }
                        case ParticleMode.oneshot:
                            {
                                setToInstancedParticleMode(true);

                                loopingParticleModeToggle.isOn = false;
                                oneshotParticleModeToggle.isOn = true;

                                break;
                            }
                        default:
                            {
                                print("Unknown case.");
                                break;
                            }
                    }

                    // ...

                    setLighting(lighting);
                    setAdvancedRendering(advancedRendering);

                    lightingToggle.isOn = lighting;
                    advancedRenderingToggle.isOn = advancedRendering;

                    // ...

                    levelToggles =
                        levelTogglesContainer.GetComponentsInChildren<Toggle>(true);

                    for (int i = 0; i < levels.Length; i++)
                    {
                        // Toggle's OnValueChanged handles
                        // level state. No need to SetActive().

                        if (i == (int)currentLevel)
                        {
                            levels[i].SetActive(true);
                            levelToggles[i].isOn = true;
                        }
                        else
                        {
                            levels[i].SetActive(false);
                            levelToggles[i].isOn = false;
                        }
                    }

                    // ...

                    updateCurrentParticleSystemNameText();
                    timeScaleSlider.onValueChanged.AddListener(onTimeScaleSliderValueChanged);

                    onTimeScaleSliderValueChanged(timeScaleSlider.value);
                }

                // ...

                public void onTimeScaleSliderValueChanged(float value)
                {
                    Time.timeScale = value;
                    timeScaleSliderValueText.text = value.ToString("0.00");
                }

                // ...

                public void setToFrontFacingCameraMode(bool set)
                {
                    if (set)
                    {
                        if (cameraMode != CameraMode.frontFacing)
                        {
                            targetCameraPosition = cameraPosition;
                        }

                        targetCameraPosition = cameraPosition;
                        targetCameraContainerRotation = Vector3.zero;

                        cameraContainerRotation = Camera.main.transform.parent.localEulerAngles;

                        cameraMode = CameraMode.frontFacing;
                    }
                }

                // ...

                public void setToInteractiveCameraMode(bool set)
                {
                    if (set)
                    {
                        if (cameraMode != CameraMode.interactive)
                        {
                            targetCameraPosition = cameraPosition2;
                        }

                        cameraContainerRotation = Camera.main.transform.parent.localEulerAngles;

                        cameraMode = CameraMode.interactive;
                    }
                }

                // ...

                public void setToPerpetualParticleMode(bool set)
                {
                    if (set)
                    {
                        oneshotParticleSystems.clear();

                        loopingParticleSystems.gameObject.SetActive(true);
                        oneshotParticleSystems.gameObject.SetActive(false);

                        particleSpawnInstructionText.gameObject.SetActive(false);

                        particleMode = ParticleMode.looping;

                        updateCurrentParticleSystemNameText();
                    }
                }

                // ...

                public void setToInstancedParticleMode(bool set)
                {
                    if (set)
                    {
                        loopingParticleSystems.gameObject.SetActive(false);
                        oneshotParticleSystems.gameObject.SetActive(true);

                        particleSpawnInstructionText.gameObject.SetActive(true);

                        particleMode = ParticleMode.oneshot;

                        updateCurrentParticleSystemNameText();
                    }
                }

                // ...

                public void setLevel(Level level)
                {
                    for (int i = 0; i < levels.Length; i++)
                    {
                        if (i == (int)level)
                        {
                            levels[i].SetActive(true);
                        }
                        else
                        {
                            levels[i].SetActive(false);
                        }
                    }

                    currentLevel = level;
                }

                // ...

                public void setLevelFromToggle(Toggle toggle)
                {
                    if (toggle.isOn)
                    {
                        setLevel((Level)System.Array.IndexOf(levelToggles, toggle));
                    }
                }

                // ...

                public void setLighting(bool value)
                {
                    lighting = value;

                    loopingParticleSystems.setLights(value);
                    oneshotParticleSystems.setLights(value);
                }

                // ...

                public void setAdvancedRendering(bool value)
                {
                    advancedRendering = value;
                    
                    postEffectsCamera.gameObject.SetActive(value);

                    UICamera.hdr = value;
                    mainCamera.hdr = value;

                    if (value)
                    {
                        QualitySettings.SetQualityLevel(32, false);

                        UICamera.renderingPath = RenderingPath.UsePlayerSettings;
                        mainCamera.renderingPath = RenderingPath.UsePlayerSettings;
                    }
                    else
                    {
                        QualitySettings.SetQualityLevel(0, false);

                        UICamera.renderingPath = RenderingPath.VertexLit;
                        mainCamera.renderingPath = RenderingPath.VertexLit;
                    }

                    for (int i = 0; i < mainCameraPostEffects.Length; i++)
                    {
                        mainCameraPostEffects[i].enabled = value;
                    }
                }

                // ...

                void Update()
                {
                    // Get targets.

                    if (cameraMode == CameraMode.interactive)
                    {
                        if (!Input.GetKey(KeyCode.LeftShift))
                        {
                            targetCameraPosition.y += Input.GetAxis("Vertical") * cameraSpeed;
                            targetCameraPosition.y = Mathf.Clamp(targetCameraPosition.y, cameraAngleLimits.x, cameraAngleLimits.y);
                        }
                        else
                        {
                            targetCameraPosition.z += Input.GetAxis("Vertical") * cameraSpeed;
                        }

                        targetCameraContainerRotation.y += Input.GetAxis("Horizontal") * cameraSpeed;
                    }

                    // Camera position.
                    
                    Camera.main.transform.localPosition = Vector3.Lerp(
                        Camera.main.transform.localPosition, targetCameraPosition, Time.deltaTime / cameraLerpTime);

                    // Camera container rotation.

                    cameraContainerRotation = Vector3.Lerp(
                        cameraContainerRotation, targetCameraContainerRotation, Time.deltaTime / cameraContainerLerpTime);

                    Camera.main.transform.parent.localEulerAngles = cameraContainerRotation;

                    // Look at origin.

                    Camera.main.transform.LookAt(Vector3.zero);

                    // Scroll through systems.

                    if (Input.GetAxis("Mouse ScrollWheel") < 0)
                    {
                        next();
                    }
                    else if (Input.GetAxis("Mouse ScrollWheel") > 0)
                    {
                        previous();
                    }

                    // Random prefab while holding key.

                    else if (Input.GetKey(KeyCode.R))
                    {
                        if (particleMode == ParticleMode.oneshot)
                        {
                            oneshotParticleSystems.randomize();
                            updateCurrentParticleSystemNameText();

                            // If also holding down, auto-spawn at random point.

                            if (Input.GetKey(KeyCode.T))
                            {
                                oneshotParticleSystems.instantiateParticlePrefabRandom();
                            }
                        }
                    }

                    // Left-click to spawn once.
                    // Right-click to continously spawn.

                    if (particleMode == ParticleMode.oneshot)
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            oneshotParticleSystems.instantiateParticlePrefab(Input.mousePosition);
                        }
                        if (Input.GetMouseButton(1))
                        {
                            oneshotParticleSystems.instantiateParticlePrefab(Input.mousePosition);
                        }
                    }
                }

                // ...

                void LateUpdate()
                {
                    // Update particle count display.

                    particleCountText.text = "PARTICLE COUNT: ";

                    if (particleMode == ParticleMode.looping)
                    {
                        particleCountText.text += loopingParticleSystems.getParticleCount().ToString();
                    }
                    else if (particleMode == ParticleMode.oneshot)
                    {
                        particleCountText.text += oneshotParticleSystems.getParticleCount().ToString();
                    }
                }

                // ...

                void updateCurrentParticleSystemNameText()
                {
                    if (particleMode == ParticleMode.looping)
                    {
                        currentParticleSystemText.text = loopingParticleSystems.getCurrentPrefabName(true);
                    }
                    else if (particleMode == ParticleMode.oneshot)
                    {
                        currentParticleSystemText.text = oneshotParticleSystems.getCurrentPrefabName(true);
                    }
                }

                // ...

                public void next()
                {
                    if (particleMode == ParticleMode.looping)
                    {
                        loopingParticleSystems.next();
                    }
                    else if (particleMode == ParticleMode.oneshot)
                    {
                        oneshotParticleSystems.next();
                    }

                    updateCurrentParticleSystemNameText();
                }

                public void previous()
                {
                    if (particleMode == ParticleMode.looping)
                    {
                        loopingParticleSystems.previous();
                    }
                    else if (particleMode == ParticleMode.oneshot)
                    {
                        oneshotParticleSystems.previous();
                    }

                    updateCurrentParticleSystemNameText();
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

// =================================	
// --END-- //
// =================================
