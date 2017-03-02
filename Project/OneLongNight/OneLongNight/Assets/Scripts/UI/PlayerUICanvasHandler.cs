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

    [Header("Stamina UI Elements")]
    [SerializeField]
    private Image staminaBarImage;

    private void OnEnable()
    {
        EventManager.StartListening(Events.NewShieldPickup, ChangedShield);
    }

    private void OnDisable()
    {
        EventManager.StopListening(Events.NewShieldPickup, ChangedShield);
    }

    private void ChangedShield()
    {
        //Change Image
        shieldImage.sprite = StatePatternPlayableCharacter.item.Item.itemIcon;
        //Change Durability
        float currentDurability = PCItemInventoryHandler.ShieldDurability;        
        float fillAmount = currentDurability * 0.01f;        
        shieldDurabilityBar.fillAmount = fillAmount;
    }

    private void ChangeWeaponImage()
    {
        weaponImage.sprite = StatePatternPlayableCharacter.WeaponPickUp.Weapon.WeaponIcon;
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
