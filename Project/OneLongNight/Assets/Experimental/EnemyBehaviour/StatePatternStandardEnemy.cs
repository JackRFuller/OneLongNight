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
    [SerializeField]
    private Texture[] materialTexs;
    [SerializeField]
    private SkinnedMeshRenderer characterMesh;
    private Material currentMat;

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
    [HideInInspector] public SEStaggerState staggerState;
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

    [Header("Field of View")]
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
    [Range(0,360)]
    private float viewAngle;
    public float ViewAngle
    {
        get
        {
            return viewAngle;
        }
    }

    //Combat========================================================================================================
    [Header("Combat")]
    [SerializeField]
    private LayerMask targetMask;
    public LayerMask TargetMask
    {
        get
        {
            return targetMask;
        }
    }
    [SerializeField]
    private LayerMask obstacleMask;
    public LayerMask ObstableMask
    {
        get
        {
            return obstacleMask;
        }
    }
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
    [SerializeField]
    private float attackCooldownTime;
    public float AttackCooldownTime
    {
        get
        {
            return attackCooldownTime;
        }
    }
    [Range(0, 1)]
    private float staggeredDirection;
    public float StaggeredDirection
    {
        get
        {
            return staggeredDirection;
        }
    }

    [Header("Item Drops")]
    [SerializeField]
    [Range(0,100)]
    private int chanceOfItemDrop;
    [SerializeField]
    [Range(0, 100)]
    private int chanceOfTwoItemDrops;
    [SerializeField]
    private DroppedItem[] itemDrops;

    private void Start()
    {
        SetUpEnemy();
    }

    public void SetUpEnemy()
    {
        currentMat = characterMesh.material;

        currentMat.mainTexture = materialTexs[(Random.Range(0, materialTexs.Length))];

        enemyMeshTransform.localPosition = Vector3.zero;
        
        if(!enemyAnim.isInitialized)
        {
            enemyAnim.Rebind();
        }

        //Set UI Attributes
        enemyUI.SetHealthAttributes(maxHealth);

        //Set Attributes
        enemyWeapon.wielder = this.transform;
        enemyWeapon.weaponDamage = attackDamage;
        health = maxHealth;

        navAgent = this.GetComponent<NavMeshAgent>();
        navAgent.avoidancePriority = Random.Range(0, 100);

        enemyWeapon.weaponDamage = attackDamage;

        //Set Enemy Weapon Layer
        enemyWeapon.gameObject.layer = LayerMask.NameToLayer("Weapon");

        //Get PC
        target = PCAttributes.Instance.transform;

        //Create States
        moveState = new SEMoveState(this);
        attackState = new SEAttackState(this);
        staggerState = new SEStaggerState(this);
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

    private void HitByPlayer(HitInfo hitInfo)
    {
        //Turn Off Weapon Collider
        enemyWeapon.GetComponent<Collider>().enabled = false;

        health -= hitInfo.damage;
        enemyUI.UpdateHealthBar(health);

        if (health <= 0)
        {
            CalculateItemDrops();
            CurrentState = deathState;
        }
        else
        {
            CameraScreenShake.Instance.TestShake();
            //Stagger
            //Determine Side
            Vector3 relativePoint = transform.InverseTransformPoint(hitInfo.hitDirection);

            if (relativePoint.x < 0)
                staggeredDirection = 0;

            if (relativePoint.x >= 0)
                staggeredDirection = 1;

            //Set Staggered State
            currentState = staggerState;
        }
    }

    private void BlockedByEnemy()
    {
        //Set Staggered State
        currentState = staggerState;
    }

    private void CalculateItemDrops()
    {
        //Check if we're dropping an item
        int ranValue = Random.Range(0, 100);
        if(ranValue <= chanceOfItemDrop)
        {
            //Determine how many items we are dropping
            ranValue = Random.Range(0, 100);

            int numberOfItemDrops = 1;

            if(ranValue <= chanceOfTwoItemDrops)
            {
                numberOfItemDrops = 2;
            }

            int weightTotal = 0;

            List<int> percentageWeightings = new List<int>();    

            //Determine Items
            for(int i = 0; i < itemDrops.Length; i++)
            {
                percentageWeightings.Add(itemDrops[i].chanceOfDrop);
                weightTotal += itemDrops[i].chanceOfDrop;
            }

            int result = 0;
            int total = 0;

            int randValue = Random.Range(0, weightTotal);
            for(result = 0; result < percentageWeightings.Count; result++)
            {
                total += percentageWeightings[result];
                if(total > randValue)
                {
                    
                    break;
                }
            }
        }
    }
    

    #region Field Of View

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if(!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public bool CanAttack()
    {
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, ViewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
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

        return false;
    }

    #endregion



}
