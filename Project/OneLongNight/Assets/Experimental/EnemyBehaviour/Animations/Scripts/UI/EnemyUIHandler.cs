using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUIHandler : BaseMonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField]
    private GameObject healthObjs;
    [SerializeField]
    private Image healthBar;

    private float maxHealth;
    

    public void SetHealthAttributes(float _maxHealth)
    {
        healthBar.fillAmount = 1.0f;
        healthObjs.SetActive(false);
        maxHealth = _maxHealth;
    }

    public void UpdateHealthBar(float newHealth)
    {
        if (!healthObjs.activeInHierarchy)
            healthObjs.SetActive(true);

        float fillAmount = newHealth / maxHealth;
        
        healthBar.fillAmount = fillAmount;

        if(fillAmount <= 0)
        {
            healthObjs.SetActive(false);
        }

    }

    
}
