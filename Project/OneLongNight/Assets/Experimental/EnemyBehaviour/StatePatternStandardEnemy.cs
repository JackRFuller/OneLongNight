using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StatePatternStandardEnemy : BaseMonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private Transform enemyMeshTransform;
    [SerializeField]
    private EnemyUIHandler enemyUI;
    [SerializeField]
    private EnemyWeaponHandler enemyWeapon;
    [SerializeField]
    private Animator enemyAnim;
    public Animator EnemyAnim
    {
        get
        {
            return enemyAnim;
        }
    }
    private NavMeshAgent navAgent;
    public NavMeshAgent NavAgent
    {
        get
        {
            return navAgent;
        }
    }

    //States========================================================================================================

    private IEnemyState currentState;
    public IEnemyState CurrentState
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

    private IEnemyState lastState;

    [HideInInspector] public SEMoveState moveState;
    [HideInInspector] public SEAttackState attackState;
    [HideInInspector] public SEDeathState deathState;

    //Movement======================================================================================================

    [Header("Movement")]
    [SerializeField]
    private float movementSpeed;
    public float MovementSpeed
    {
        get
        {
            return movementSpeed;
        }
    }

    private Transform target; //IS PC
    public Transform Target
    {
        get
        {
            return target;
        }
    }

    //Combat========================================================================================================
    [Header("Combat")]
    [SerializeField]
    private float maxHealth;
    private float health;
    public float Health
    {
        get
        {
            return health;
        }
    }
    [SerializeField]
    private float distanceToPCToAttack;
    public float DistanceToPCToAttack
    {
        get
        {
            return distanceToPCToAttack;
        }
    }
    [SerializeField]
    private float attackDamage;
    

    private void Start()
    {
        SetUpEnemy();
    }

    public void SetUpEnemy()
    {
        enemyMeshTransform.localPosition = Vector3.zero;
        
        if(!enemyAnim.isInitialized)
        {
            enemyAnim.Rebind();
        }

        //Set UI Attributes
        enemyUI.SetHealthAttributes(maxHealth);

        //Set Attributes
        enemyWeapon.weaponDamage = attackDamage;
        health = maxHealth;

        navAgent = this.GetComponent<NavMeshAgent>();

        enemyWeapon.weaponDamage = attackDamage;

        //Get PC
        target = PCAttributes.Instance.transform;

        //Create States
        moveState = new SEMoveState(this);
        attackState = new SEAttackState(this);
        deathState = new SEDeathState(this);

        currentState = moveState;
    }

    public override void UpdateNormal()
    {
        if (currentState != lastState)
        {
            currentState.OnEnterState();

            lastState = currentState;
        }
        else
        {
            if (currentState != null)
                currentState.OnUpdateState();
        }
    }

    private void HitByPlayer(float _damage)
    {
        UpdateHealth(_damage);
    }

    private void UpdateHealth(float _damageTaken)
    {
        health -= _damageTaken;
        enemyUI.UpdateHealthBar(health);

        if(health <= 0)
        {
            CurrentState = deathState;
        }

    }



}
