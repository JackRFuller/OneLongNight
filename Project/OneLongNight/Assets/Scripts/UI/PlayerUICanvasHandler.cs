using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUICanvasHandler : BaseMonoBehaviour
{
    [Header("Consumable UI Elements")]
    [SerializeField]
    private Text numOfHealthPotions;

    [Header("Shield UI Elements")]
    [SerializeField]
    private Image shieldImage;
    [SerializeField]
    private Image shieldDurabilityBar;

    [Header("Weapon UI Elements")]
    [SerializeField]
    private Image weaponImage;
    [SerializeField]
    private Image weaponDurabilityBar;  

    [Header("Stamina UI Elements")]
    [SerializeField]
    private Image staminaBarImage;

    [Header("Health Bar Elements")]
    [SerializeField]
    private Image healthBarFillImage;
    [SerializeField]
    private Image healthBarDamageTaken;

    //Health Bar Lerping Variables
    private bool isLerpingHealthBar;
    private float targetFill;
    private float startingFill;
    private float healthBarLerpSpeed = 1f;
    private float timeStartedLerpingHealthBar;

    [Header("Death Message")]
    [SerializeField]
    private CanvasGroup deathMessage;
    private float startAlpha = 0;
    private float targetAlpha = 1;
    private float deathMessageLerpSpeed = 4f;
    private float timeStartedLerpingDM;
    private bool isLerpingDM;

    private void OnEnable()
    {
        //PC Attribute Bars
        EventManager.StartListening(Events.HitByEnemy, UpdateHealthBar);
        EventManager.StartListening(Events.PlayerDied, SetupDeathMessage);
        EventManager.StartListening(Events.RegeneratingHealth, RegenerateHealth);

        EventManager.StartListening(Events.NewShieldPickup, ChangedShield);
        EventManager.StartListening(Events.NewWeaponPickup, ChangeWeaponImage);

        //Consumables
        EventManager.StartListening(Events.PickUpHealthPotion, HealthPotionPickedUp);
    }

    private void OnDisable()
    {
        EventManager.StopListening(Events.HitByEnemy, UpdateHealthBar);
        EventManager.StopListening(Events.PlayerDied, SetupDeathMessage);
        EventManager.StopListening(Events.RegeneratingHealth, RegenerateHealth);

        EventManager.StopListening(Events.NewShieldPickup, ChangedShield);
        EventManager.StopListening(Events.NewWeaponPickup, ChangeWeaponImage);

        //Consumables
        EventManager.StopListening(Events.PickUpHealthPotion, HealthPotionPickedUp);
    }

    private void ChangedShield()
    {
        //Change Image
        shieldImage.sprite = PCItemInventoryHandler.Item.itemIcon;

        //Change Durability
        float currentDurability = PCItemInventoryHandler.ShieldDurability;        
        float fillAmount = currentDurability * 0.01f;        
        shieldDurabilityBar.fillAmount = fillAmount;
    }

    private void ChangeWeaponImage()
    {
        weaponImage.sprite = PCItemInventoryHandler.Item.itemIcon;
    }

    public override void UpdateNormal()
    {
        UpdateStaminaBar();

        if (isLerpingHealthBar)
            LerpHealthDamageBar();

        if (isLerpingDM)
            LerpDeathMessage();
    }

    private void HealthPotionPickedUp()
    {
        numOfHealthPotions.text = "x" + PCItemInventoryHandler.HealthPotionCount.ToString();
    }

    private void UpdateStaminaBar()
    {
        float fillPercent = PCAttributes.Instance.PCStamina * 0.01f;      
        staminaBarImage.fillAmount = fillPercent;
    }

    private void RegenerateHealth()
    {
        float health = PCAttributes.Health;
        //Change Fill Amount
        float fillAmount = health / 100.0f;
        healthBarFillImage.fillAmount = fillAmount;
    }

    private void UpdateHealthBar()
    {
        //Set Health Bar To Current Health
        float health = PCAttributes.Health;

        //Change Fill Amount
        float fillAmount = health / 100.0f;
        healthBarFillImage.fillAmount = fillAmount;

        StartCoroutine(WaitToStartHealthBarLerp(fillAmount));
    }

    IEnumerator WaitToStartHealthBarLerp(float fillAmount)
    {
        yield return new WaitForSeconds(1.0f);
        // Setup Health Damage Lerp
        startingFill = healthBarDamageTaken.fillAmount;
        targetFill = fillAmount;
        timeStartedLerpingHealthBar = Time.time;
        isLerpingHealthBar = true;

    }

    private void LerpHealthDamageBar()
    {
        float timeSinceStarted = Time.time - timeStartedLerpingHealthBar;
        float percentageComplete = timeSinceStarted / healthBarLerpSpeed;

        float fillAmount = Mathf.Lerp(startingFill, targetFill, percentageComplete);

        healthBarDamageTaken.fillAmount = fillAmount;

        if(percentageComplete >= 1.0f)
        {
            isLerpingHealthBar = false;
        }
    }

    //Death Message
    private void SetupDeathMessage()
    {
        if(!isLerpingDM)
        {
            timeStartedLerpingDM = Time.time;
            isLerpingDM = true;
        }

        
    }

    private void LerpDeathMessage()
    {
        float timeSInceStarted = Time.time - timeStartedLerpingDM;
        float percentageComplete = timeSInceStarted / deathMessageLerpSpeed;

        float alpha = Mathf.Lerp(startAlpha, targetAlpha, percentageComplete);

        deathMessage.alpha = alpha;

        if(percentageComplete >= 1.0f)
        {
            isLerpingDM = false;
        }
    }
}
