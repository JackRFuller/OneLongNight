using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCStateController : MonoBehaviour
{

    [Header("Components")]
    [SerializeField]
    private CharacterController controller;
    public CharacterController Controller
    {
        get
        {
            return controller;
        }
    }

    [Header("States")]
    public PCMovementState movementState;
    private IPlayableCharacterState currentState;
    public IPlayableCharacterState CurrentState
    {
        get
        {
            return currentState;
        }
    }

    public Quaternion newestRot;
    public Quaternion oldRot;

    int frameCount = 0;


    //Statess

    //Input
    private Vector3 movementVector;
    public Vector3 MovementVector
    {
        get
        {
            return movementVector;
        }
    }
    public Quaternion playerRot;

	// Use this for initialization
	void Start ()
    {
        controller = this.GetComponent<CharacterController>();

        //Setup States
        movementState = new PCMovementState(this);

        //Set Current State
        currentState = movementState;

        //StartCoroutine(UpdatePlayerRotation());
	}

    public void Update()
    {
        GetInput();
        currentState.OnUpdateState();
    }
   
    void GetInput()
    {
        frameCount++;

        if(frameCount == 2)
        {
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");

            movementVector = new Vector3(x, 0, z);
            frameCount = 0;
        }
    }

    IEnumerator UpdatePlayerRotation()
    {
        while(true)
        {
            yield return new WaitForEndOfFrame();
            if(movementVector != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(movementVector);
        }
    }
}
