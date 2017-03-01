using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUICanvasHandler : BaseMonoBehaviour
{
    [SerializeField]
    private Image weaponImage;

    [Header("Stamina UI Elements")]
    [SerializeField]
    private Image staminaBarImage;

    private void OnEnable()
    {
        EventManager.StartListening(Events.ChnagedWeapon, ChangeWeaponImage);
    }

    private void OnDisable()
    {
        EventManager.StopListening(Events.ChnagedWeapon, ChangeWeaponImage);
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
