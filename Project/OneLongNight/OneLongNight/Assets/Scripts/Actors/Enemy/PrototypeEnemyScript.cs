using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class PrototypeEnemyScript : BaseMonoBehaviour
{
    [SerializeField]
    private Animator animController;

    //Navigation
    [Header("Navigation")]
    [SerializeField]
    private float distToPlayerToStop;
    private NavMeshAgent navAgent;
    private Transform target;

    [Header("UI Elements")]
    [SerializeField]
    private GameObject enemyCanvas;
    [SerializeField]
    private Image healthBarImage;
    private float enemyHealth = 100;
    private Camera mainCamera;

    private void Start()
    {
        enemyCanvas.SetActive(false);

        mainCamera = Camera.main;

        navAgent = this.GetComponent<NavMeshAgent>();
        target = PCAttributes.Instance.transform;

        navAgent.destination = target.position;
    }

    public void ResetEnemy()
    {
        animController.SetBool("ifDead", false);
        animController.SetBool("isIdle", true);

        enemyHealth = 100.0f;
        float fillAmount = enemyHealth / 100.0f;
        healthBarImage.fillAmount = fillAmount;

        enemyCanvas.SetActive(false);
    }

    public override void UpdateNormal()
    {
        UpdateDestination();

        if(enemyCanvas.activeInHierarchy)
            TurnCanvasTowardsCamera();
    }

    private void UpdateDestination()
    {
        //Calculate 
        float dist = Vector3.Distance(this.transform.position, target.position);

        if(dist > distToPlayerToStop)
        {
            animController.SetBool("isWalking", true);
            animController.SetBool("isIdle", false);
            navAgent.speed = 1f;
            navAgent.destination = target.position;
        }
        else
        {
            animController.SetBool("isWalking", false);
            animController.SetBool("isIdle", true);
            navAgent.speed = 0;
        }
        
    }

    private void TurnCanvasTowardsCamera()
    {
        Vector3 lookAtPos = new Vector3(mainCamera.transform.position.x,
                                        enemyCanvas.transform.position.y,
                                        mainCamera.transform.position.z);

        enemyCanvas.transform.LookAt(lookAtPos);
                                        
    }

    private void HitByPlayer(float _damage)
    {
        UpdateHealth(_damage);
    }

    private void UpdateHealth(float _damageTaken)
    {
        //Debug.Log("Enemy Took Damage of " +_damageTaken);

        if (!enemyCanvas.activeInHierarchy)
            enemyCanvas.SetActive(true);


        enemyHealth -= _damageTaken;
        float fillAmount = enemyHealth / 100.0f;
        healthBarImage.fillAmount = fillAmount;


        if (enemyHealth <= 0)
        {
            StartCoroutine(TurnOffEnemy());
        }
        else
        {
            //Stagger Back
            animController.SetBool("isStaggered",true);
            navAgent.speed = 0;
            StartCoroutine(TurnOffStagger());
            
        }

    }

    IEnumerator TurnOffStagger()
    {
        yield return new WaitForSeconds(0.25f);
        animController.SetBool("isStaggered", false);
    }

    IEnumerator TurnOffEnemy()
    {
        animController.SetBool("ifDead", true);
        enemyCanvas.SetActive(false);
        yield return new WaitForSeconds(2.25f);
        this.gameObject.SetActive(false);
    }
}
