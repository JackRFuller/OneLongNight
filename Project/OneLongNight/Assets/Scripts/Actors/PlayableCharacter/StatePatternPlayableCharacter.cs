using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Rewired;

public class StatePatternPlayableCharacter : BaseMonoBehaviour
{
    //Components
    private NavMeshAgent navAgent;
    public NavMeshAgent NavAgent
    {
        get
        {
            return navAgent;
        }
    }
    private Animator pcAnimator;
    public Animator PCAnimator
    {
        get
        {
            return pcAnimator;
        }
    }

    [Header("Actions")]   
    [SerializeField]
    private MovementActions rollAction;
    public MovementActions RollAction
    {
        get
        {
            return rollAction;
        }
    }

    [Header("Camera")]    
    [SerializeField]
    private Transform cameraTarget;
    public Transform CameraTarget
    {
        get
        {
             return cameraTarget;
        }
    }   

    //Item Pickups================================================================
    [Header("Items")]
    [SerializeField]
    private PCItemInventoryHandler itemHandler; //Cycles through all of the logic for picking up and placing items        

    //Weapons======================================================================== 
    private bool hasShield;
    public bool HasShield
    {
        get
        {
            return hasShield;
        }
        set
        {
            hasShield = value;
        }
    }

    //States=========================================================================

    //Core States
    private IPlayableCharacterState currentState;
    public IPlayableCharacterState CurrentState
    {
        get
        {
            return currentState;
        }
        set
        {
            currentState = value;
        }
    }
    private IPlayableCharacterState lastState;

    //States - Movement
    [HideInInspector] public PCIdleState idleState;    
    [HideInInspector] public PCMoveState moveState;    
    [HideInInspector] public PCRollState rollState;
    [HideInInspector] public PCPickupState pickUpState;
    [HideInInspector] public PCStaggerState staggerState;
    [HideInInspector] public PCAttackState attackState;
    [HideInInspector] public PCDeathState deathState;

    //Combat=========================================================================
    [Header("Combat")]
    [SerializeField]
    private float staggerCooldown; //How soon after being hit can the player move
    public float StaggerCooldown
    {
        get
        {
            return staggerCooldown;
        }
    }    
    public static Transform attackingEnemy;
    public static Vector3 hitDirection;
    public static float WeaponRange;

    [Header("Blocking Attributes")]
    [SerializeField]
    private float viewRadius;
    public float ViewRadius
    {
        get
        {
            return viewRadius;
        }
    }
    [SerializeField]
    private float viewAngle;
    public float ViewAngle
    {
        get
        {
            return viewAngle;
        }
    }
    [SerializeField]
    private LayerMask targetMask;
    [SerializeField]
    private LayerMask obstacleMask;
    private Transform attacker;

    //Inputs =========================================================================
    [SerializeField]
    private Transform targetMarker;
        
    //Movement
    private Transform enemyTarget;
    public Transform EnemyTarget
    {
        get
        {
            return enemyTarget;
        }
        set
        {
            enemyTarget = value;
        }
    }
    private Vector3 targetPosition;
    public Vector3 TargetPosition
    {
        get
        {
            return targetPosition;
        }
    }
    private bool hasPickupTarget; //Used to control whether the player is moving towards a pickup target
    public bool HasPickupTarget
    {
        get
        {
            return hasPickupTarget;
        }
        set
        {
            hasPickupTarget = value;
        }
    }
    private bool hasTargetPosition;
    public bool HasTargetPosition
    {
        get
        {
            return hasTargetPosition;
        }
        set
        {
            hasTargetPosition = value;
        }
    }  

    private void OnEnable()
    {
        EventManager.StartListening(Events.PlayerDied, PlayerHasDied);
    }

    private void OnDisable()
    {
        EventManager.StopListening(Events.PlayerDied, PlayerHasDied);
    }

    private void Start()
    {
        //Get Components
        pcAnimator = this.GetComponent<Animator>();
        navAgent = this.GetComponent<NavMeshAgent>();
       
        //Get States - Movement
        idleState = new PCIdleState(this);
        moveState = new PCMoveState(this);
        rollState = new PCRollState(this);        
        pickUpState = new PCPickupState(this);
        staggerState = new PCStaggerState(this);
        deathState = new PCDeathState(this);

        attackState = new PCAttackState(this);
        
        //Set Starting State
        currentState = idleState;
    }

    public override void UpdateNormal()
    {
        //GetInputs();
        GetMouseInput();

        UpdateCurrentState();
    }

    private void UpdateCurrentState()
    {
		if(currentState != lastState)
		{
			currentState.OnEnterState();

			lastState = currentState;
            //Debug.Log("New State " + currentState);
		}
		else
		{
			if(currentState != null)
				currentState.OnUpdateState();
		}
    }
    
    private void PlayerHasDied()
    {
        currentState = deathState;
    }

    public void HitByEnemy(HitInfo hitInfo)
    {
        attacker = hitInfo.attacker;
        enemyTarget = null;
        hasTargetPosition = false;
        hasPickupTarget = false;
        HurtByEnemy(hitInfo);

        ////Check if We're Blocking
        //if (isBlocking)
        //{
        //    //Check if Attacker is in field of view
        //    if(CanBlock())
        //    {
        //        Debug.Log("Blocked Successfully");
        //        CameraScreenShake.Instance.TestShake();
        //        attacker.SendMessage("BlockedByEnemy", SendMessageOptions.DontRequireReceiver);
        //        pcAnimator.SetTrigger("hasBlocked");
        //    }
        //    else
        //    {
        //        HurtByEnemy(hitInfo);
        //    }
        //}
        //else
        //{
            
        //}
    }

    /// <summary>
    /// Called if the player doesn't successfully block
    /// </summary>
    void HurtByEnemy(HitInfo hitInfo)
    {
        //Take Away Health
        PCAttributes.Health -= hitInfo.damage;

        //Determine Hit Direction
        CameraScreenShake.Instance.TestShake();
        EventManager.TriggerEvent(Events.HitByEnemy);

        if (PCAttributes.Health <= 0)
        {
            EventManager.TriggerEvent(Events.PlayerDied);
            this.GetComponent<Collider>().enabled = false;
        }
        else
        {
            hitDirection = hitInfo.hitDirection;
            attackingEnemy = hitInfo.attacker;
            currentState = staggerState;
            EventManager.TriggerEvent(Events.PlayerStaggered);
        }
    }

    #region Field Of View

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y + 45;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public bool CanBlock()
    {
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, ViewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            if(attacker.gameObject == targetsInViewRadius[i].gameObject)
            {
                Debug.Log("Found Attacker");

                Transform target = targetsInViewRadius[i].transform;

                Vector3 dirToTarget = (target.position - transform.position).normalized;

                if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
                {
                    float dstToTarget = Vector3.Distance(transform.position, target.position);

                    if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                Debug.Log("Could Not Find Attacker");
                return false;
            }
        }

        return false;
    }

    #endregion

#region Inputs

    private void GetMouseInput()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            //Get Left Click
            if(Input.GetMouseButtonUp(0))
            {
                if (hit.collider.tag == "Ground" || hit.collider.tag == "Pickup")
                {
                    targetPosition = new Vector3(hit.point.x,
                                                 this.transform.position.y,
                                                 hit.point.z);

                    targetMarker.position = targetPosition;

                    hasTargetPosition = true;
                    if(hit.collider.tag == "Pickup")
                    {
                        hasPickupTarget = true;
                        PCItemInventoryHandler.foundItem = hit.collider.GetComponent<ItemPickup>();
                    }

                    enemyTarget = null;
                }
                else if(hit.collider.tag == "Enemy")
                {
                    if(currentState != attackState)
                    {
                        enemyTarget = hit.transform;
                        HasTargetPosition = true;
                    }
                }
            }

            //Get Right Click
            if(Input.GetMouseButton(1))
            {
                //Roll
                if(hit.collider.tag == "Ground")
                {
                    //If We're Not Currently Rolling
                    if(currentState != rollState && currentState != staggerState)
                    {
                        //If We've got enough Stamina
                        if(PCAttributes.Instance.CheckIfPCHasEnoughStamina(rollAction.ActionCost))
                        {
                            if(currentState == attackState)
                            {
                                if(PCItemInventoryHandler.CurrentWeapon.weaponWeight == ItemData.WeaponWeight.Light)
                                {
                                    targetPosition = new Vector3(hit.point.x,
                                                this.transform.position.y,
                                                hit.point.z);
                                    CurrentState = rollState;
                                }
                            }
                            else
                            {
                                targetPosition = new Vector3(hit.point.x,
                                                this.transform.position.y,
                                                hit.point.z);
                                CurrentState = rollState;
                            }
                        }
                    }
                }
            }
        }
    }

    private void GetUseItemInput()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (PCItemInventoryHandler.HealthPotionCount > 0)
            {
                //Get Health Potion Use
                if (CurrentState != rollState && CurrentState != attackState)
                {
                    //Instantiate Health Zone
                    GameObject healthZone = Instantiate(ItemManager.Instance.HealthZone) as GameObject;
                    healthZone.transform.position = new Vector3(this.transform.position.x, 0, this.transform.position.z);

                    //Remove Health Potion
                    PCItemInventoryHandler.Instance.UsedHealthPotion();
                }
            }
        }
    }

#endregion
}
