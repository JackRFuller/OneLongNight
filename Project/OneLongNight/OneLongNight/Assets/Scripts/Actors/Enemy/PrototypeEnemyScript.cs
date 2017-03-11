using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrototypeEnemyScript : BaseMonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField]
    private Image healthBarImage;
    private float enemyHealth = 100;

    private void HitByPlayer(float _damage)
    {
        UpdateHealth(_damage);
    }

    private void UpdateHealth(float _damageTaken)
    {
        Debug.Log("Enemy Took Damage of " +_damageTaken);

        enemyHealth -= _damageTaken;
        float fillAmount = enemyHealth / 100.0f;
        healthBarImage.fillAmount = fillAmount;


        if (enemyHealth <= 0)
        {
            TurnOffEnemy();
        }

    }

    private void TurnOffEnemy()
    {
        this.gameObject.SetActive(false);
    }
}
