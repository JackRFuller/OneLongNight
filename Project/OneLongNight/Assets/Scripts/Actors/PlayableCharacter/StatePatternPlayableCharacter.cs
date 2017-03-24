using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class StatePatternPlayableCharacter : BaseMonoBehaviour
{
    //Components
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

    [Header("Movement")]
    [SerializeField]
    private Transform target;
    public Transform Target
    {
        get
        {
            return target;
        }
    }
    [SerializeField]
    private Transform cameraTarget;
    public Transform CameraTarget
    {
        get
        {
             return cameraTarget;
        }
    }
    public Transform targetHolder;

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
    private Player player;
    //Movement
    private int frameWait;

    private Vector3 movementVector;
    public Vector3 MovementVector
    {
        get
        {
            return movementVector;
        }
    }

    private bool isBlocking;
    public bool IsBlocking
    {
        get
        {
            return isBlocking;
        }
    }    

    private bool isRolling;
    public bool IsRolling
    {
        get
        {
            return isRolling;
        }
    }

    private bool isAttacking;
    public bool IsAttacking
    {
        get
        {
            return isAttacking;
        }
    }

    private bool isPickingUp;
    public bool IsPickingUp
    {
        get
        {
            return isPickingUp;
        }
    }

    

    private void OnEnable()
    {
        EventManager.StartListening(Events.PlayerStaggered, PlayerIsStaggered);
        EventManager.StartListening(Events.PlayerDied, PlayerHasDied);
    }

    private void OnDisable()
    {
        EventManager.StopListening(Events.PlayerStaggered, PlayerIsStaggered);
        EventManager.StopListening(Events.PlayerDied, PlayerHasDied);
    }

    private void Start()
    {
        player = ReInput.players.GetPlayer(0);

        pcAnimator = this.GetComponent<Animator>();

        //Movement
        target.position = this.transform.position;

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
        GetInputs();

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

    private void PlayerIsStaggered()
    {
        currentState = staggerState;
    } 

    private void PlayerHasDied()
    {
        currentState = deathState;
    }

    public void HitByEnemy(HitInfo hitInfo)
    {
        attacker = hitInfo.attacker;

        //Check if We're Blocking
        if(isBlocking)
        {
            //Check if Attacker is in field of view
            if(CanBlock())
            {
                Debug.Log("Blocked Successfully");
                CameraScreenShake.Instance.TestShake();
                attacker.SendMessage("BlockedByEnemy", SendMessageOptions.DontRequireReceiver);
                pcAnimator.SetTrigger("hasBlocked");
            }
            else
            {
                HurtByEnemy(hitInfo);
            }
        }
        else
        {
            HurtByEnemy(hitInfo);
        }

       
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

    private void GetInputs()
    {
        if(CurrentState != deathState)
        {            
            GetMovementInput();

            GetUseItemInput();

            GetAttackInput();

            GetRollInput();

            GetPickUpInput();

            GetBlockInput();
        }
    }

    private void GetMovementInput()
    {
        //Get Directional Movement
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        movementVector = new Vector3(x, 0, z);
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

    private void GetAttackInput()
    {
        //Get Combat Input & Check We're not Rolling or Picking Up
        if (player.GetButton("Attack") || Input.GetMouseButton(0))
        {
            if (CurrentState != rollState && CurrentState != pickUpState)
            {
                CurrentState = attackState;
                isAttacking = true;
            }
            else
            {
                isAttacking = false;
            }
        }
        else
        {
            isAttacking = false;
        }
    }

    private void GetPickUpInput()
    {
        //Get Pickup Input
        if (player.GetButtonDown("Interact"))
        {
            if (PCItemInventoryHandler.PickUpFinished)
            {
                if (PCItemInventoryHandler.foundItem)
                {
                    if(CurrentState != pickUpState && CurrentState != rollState && CurrentState != attackState && CurrentState != staggerState)
                    {
                        CurrentState = pickUpState;
                        itemHandler.PickUpItem();
                    }                    
                }
            }
        }
        else
        {
            isPickingUp = false;
        }
    }

    private void GetRollInput()
    {
        //Get Roll Input
        if (player.GetButtonDown("Roll"))
        {
            if (CurrentState != attackState && CurrentState != staggerState && CurrentState != pickUpState && CurrentState != rollState)
            {
                CurrentState = rollState;
            }
        }
    }

    private void GetBlockInput()
    {
        //Get Block Input
        if (hasShield)
        {
            if (player.GetButton("Block") || Input.GetMouseButton(1))
            {
                isBlocking = true;
            }
            else
            {
                isBlocking = false;
            }
        }
        else
        {
            isBlocking = false;
        }
    }

#endregion
}
