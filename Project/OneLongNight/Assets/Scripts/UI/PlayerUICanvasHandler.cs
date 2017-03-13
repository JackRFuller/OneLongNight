using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUICanvasHandler : BaseMonoBehaviour
{
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

    private void OnEnable()
    {
        EventManager.StartListening(Events.NewShieldPickup, ChangedShield);
        EventManager.StartListening(Events.NewWeaponPickup, ChangeWeaponImage);
    }

    private void OnDisable()
    {
        EventManager.StopListening(Events.NewShieldPickup, ChangedShield);
        EventManager.StopListening(Events.NewWeaponPickup, ChangeWeaponImage);
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
    }

    private void UpdateStaminaBar()
    {
        float fillPercent = PCAttributes.Instance.PCStamina * 0.01f;      
        staminaBarImage.fillAmount = fillPercent;
    }
}
