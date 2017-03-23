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
    private Image healthBarImage;
    [SerializeField]
    private Image healthBarDamageTakenImage;

    //Health Bar Lerping Properties
    private float startingFillAmount;
    private float targetFillAmount;
    private float timeStarted;
    private float healthBarLerpTime;
    private bool isLerpingHealthBar;
    private float healthBarLerpSpeed = 1.0f;

    private float maxHealth;
    

    public void SetHealthAttributes(float _maxHealth)
    {
        healthBarImage.fillAmount = 1.0f;
        healthBarDamageTakenImage.fillAmount = 1.0f;
        healthObjs.SetActive(false);
        maxHealth = _maxHealth;
    }

    public void UpdateHealthBar(float newHealth)
    {
        if (!healthObjs.activeInHierarchy)
            healthObjs.SetActive(true);

        float fillAmount = newHealth / maxHealth;
        startingFillAmount = healthBarImage.fillAmount;
        targetFillAmount = fillAmount;

        healthBarImage.fillAmount = fillAmount;

        timeStarted = Time.time;
        isLerpingHealthBar = true;


        if(fillAmount <= 0)
        {
            healthObjs.SetActive(false);
        }

    }

    public override void UpdateNormal()
    {
        if (isLerpingHealthBar)
            LerpDamageBar();
    }

    private void LerpDamageBar()
    {
        float timeSinceStarted = Time.time - timeStarted;
        float percentageComplete = timeSinceStarted / healthBarLerpSpeed;

        float fillAmount = Mathf.Lerp(startingFillAmount, targetFillAmount, percentageComplete);
        healthBarDamageTakenImage.fillAmount = fillAmount;

        if(percentageComplete >= 1.0f)
        {
            isLerpingHealthBar = false;
        }
    }


}
