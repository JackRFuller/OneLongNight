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
    private MovementActions sprintAction;
    public MovementActions SprintAction
    {
        get
        {
            return sprintAction;
        }
    }
    [SerializeField]
    private MovementActions slowRollAction;
    public MovementActions SlowRollAction
    {
        get
        {
            return slowRollAction;
        }
    }
    [SerializeField]
    private MovementActions fastRollAction;
    public MovementActions FastRolLAction
    {
        get
        {
            return fastRollAction;
        }
    }

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

    [Header("Shields")]
    [SerializeField]
    private GameObject shieldObject;
    private static ShieldHandler shield;
    public static ShieldHandler Shield
    {
        get
        {
            return shield;
        }
        set
        {
            shield = value;
        }
    }
    private bool hasShield;
    public bool HasShield
    {
        get
        {
            return hasShield;
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
    [HideInInspector] public PCWalkState walkState;
    [HideInInspector] public PCSprintState sprintState;
    [HideInInspector] public PCSlowRollState slowRollState;
    [HideInInspector] public PCFastRollState fastRollState;

	//States - Weapon
	[HideInInspector] public PCStandardAttackState standardAttackState;

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
        walkState = new PCWalkState(this);
        sprintState = new PCSprintState(this);
        slowRollState = new PCSlowRollState(this);
        fastRollState = new PCFastRollState(this);

		//Get States - Combat
		standardAttackState = new PCStandardAttackState(this);
        
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
		overrideController["Jog"] = newWeapon.MovementAnimation.walkAnim.clip;
		overrideController["Sprint"] = newWeapon.MovementAnimation.runAnim.clip;
		overrideController["SlowRoll"] = newWeapon.MovementAnimation.slowRollAnim.clip;
		overrideController["FastRoll"] = newWeapon.MovementAnimation.fastRollAnim.clip;

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

    /// <summary>
    /// Pickups Shield
    /// Shield Variable is set in ShieldHandler
    /// </summary>
    void SetShield()
    {
        shield.PickUpItem();
        
        hasShield = true;
        pcAnimator.SetBool("hasShield", true);

        shieldObject.SetActive(true);

        shield = null;

        EventManager.TriggerEvent(Events.ExitItemPickup);
    }
    
    void GetWeapon()
    {
        currentWeapon = weaponPickup.Weapon;
        
        weaponPickup.GetWeapon();

        weapons[weaponPickup.WeaponIndex].SetActive(true);

		OverrideAnimationClips(currentWeapon);

        EventManager.TriggerEvent(Events.ChnagedWeapon);
        EventManager.TriggerEvent(Events.ExitItemPickup);       

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
            if (shield)
                SetShield();

            if (weaponPickup)
                GetWeapon();
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

        //Get Sprint Input
        if(Input.GetMouseButton(1))
        {
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }

        //Get Directional Movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        movementVector = new Vector3(x, 0, z);
    }
}
