using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //Item Pickups================================================================
    [Header("Items")]
    [SerializeField]
    private PCItemInventoryHandler itemHandler; //Cycles through all of the logic for picking up and placing items
    public static ItemPickup item;    

    //Weapons========================================================================
    [Header("Weapons")]
    [SerializeField]
    private WeaponData startingWeapon;
    [SerializeField]
    private GameObject[] weapons;
    private WeaponData currentWeapon;
    public WeaponData CurrentWeapon
    {
        get
        {
            return currentWeapon;
        }
    }
    private static WeaponPickup weaponPickup;
    public static WeaponPickup WeaponPickUp
    {
        get
        {
            return weaponPickup;
        }
        set
        {
            weaponPickup = value;
        }
    }
    
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
    [HideInInspector] public PCBlockIdleState blockIdleState;
    [HideInInspector] public PCMoveState moveState;
    [HideInInspector] public PCBlockMoveState blockMoveState;
    [HideInInspector] public PCRollState rollState;  	

    //Inputs =========================================================================
    //Movement
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

    private bool isSprinting;
    public bool IsSprinting
    {
        get
        {
            return isSprinting;
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

    private bool isTryingToAttack;
    public bool IsTryingToAttack
    {
        get
        {
            return isTryingToAttack;
        }
    }

    private void Start()
    {
        pcAnimator = this.GetComponent<Animator>();

        //Get States - Movement
        idleState = new PCIdleState(this);
        blockIdleState = new PCBlockIdleState(this);
        moveState = new PCMoveState(this);
        rollState = new PCRollState(this);
        blockMoveState = new PCBlockMoveState(this);  
        
        //Set Starting State
        currentState = idleState;

		OverrideAnimationClips(startingWeapon);
    }

   

    //Takes New Weapon and Sets Up New Animations
    private void OverrideAnimationClips(WeaponData newWeapon)
    {
        Animator animator = GetComponent<Animator>();

        AnimatorOverrideController overrideController = new AnimatorOverrideController();
        overrideController.runtimeAnimatorController = GetEffectiveController(animator);

        //Set Up New Animation Clips - Movement
		overrideController["Idle"] = newWeapon.MovementAnimation.idleAnim.clip;
		//overrideController["Jog"] = newWeapon.MovementAnimation.walkAnim.clip;
		overrideController["Move"] = newWeapon.MovementAnimation.runAnim.clip;
		//overrideController["SlowRoll"] = newWeapon.MovementAnimation.slowRollAnim.clip;
		overrideController["Roll"] = newWeapon.MovementAnimation.fastRollAnim.clip;

		//Set up New Animation Clips - Weapons
		//overrideController["Attack1"] = newWeapon.WeaponAnimation.attackOneAnim.clip;
		//overrideController["Attack2"] = newWeapon.WeaponAnimation.attackTwoAnim.clip;
		//overrideController["Attack3"] = newWeapon.WeaponAnimation.attackThreeAnim.clip;

        animator.runtimeAnimatorController = overrideController;        
    }

    //Create New Animator
    private RuntimeAnimatorController GetEffectiveController(Animator animator)
    {
        RuntimeAnimatorController controller = animator.runtimeAnimatorController;

        AnimatorOverrideController overrideController = controller as AnimatorOverrideController;
        while (overrideController != null)
        {
            controller = overrideController.runtimeAnimatorController;
            overrideController = controller as AnimatorOverrideController;
        }

        return controller;
    }
       
    void GetWeapon()
    {
        currentWeapon = weaponPickup.Weapon;
        
        weaponPickup.GetWeapon();

        weapons[weaponPickup.WeaponIndex].SetActive(true);

		OverrideAnimationClips(currentWeapon);
        weaponPickup = null;
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
		}
		else
		{
			if(currentState != null)
				currentState.OnUpdateState();
		}

//        if(lastState != currentState)
//        {
//            if (lastState != null)
//                lastState.OnUpdateState();
//
//            lastState = currentState;
//
//            Debug.Log(currentState);
//        }
        
    }

    private void GetInputs()
    {
        //PickUp
        if(Input.GetKeyDown(KeyCode.E))
        {
            if (item)
                itemHandler.PickUpItem(item.Item);
        }


        //Get Combat Input & Check We're not Rolling
        if(!isRolling)
        {
            if(Input.GetMouseButton(0))
            {
                isTryingToAttack = true;
            }
            else
            {
                isTryingToAttack = false;
            }
        }
       

        //Get Roll Input
        if(Input.GetKeyDown(KeyCode.Space))
        {
            isRolling = true;
        }
        else
        {
            isRolling = false;
        }

        //Get Block Input
        if(hasShield)
        {
            if (Input.GetMouseButton(1))
            {
                isBlocking = true;
            }
            else
            {
                isBlocking = false;
            }
        }        

        //Get Directional Movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        movementVector = new Vector3(x, 0, z);
    }
}
