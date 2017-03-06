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
    [HideInInspector] public PCBlockIdleState blockIdleState;
    [HideInInspector] public PCMoveState moveState;
    [HideInInspector] public PCBlockMoveState blockMoveState;
    [HideInInspector] public PCRollState rollState;
    [HideInInspector] public PCPickupState pickUpState;

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

    private bool isPickingUp;
    public bool IsPickingUp
    {
        get
        {
            return isPickingUp;
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
        pickUpState = new PCPickupState(this);
        
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
        //Get Pickup Input
        if(Input.GetKey(KeyCode.E))
        {
            if(PCItemInventoryHandler.PickUpFinished)
            {
                if (PCItemInventoryHandler.foundItem)
                {
                    isPickingUp = true;
                    itemHandler.PickUpItem();
                }
            }
        }
        else
        {
            isPickingUp = false;
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
