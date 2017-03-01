using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInstructionCanvasHandler : BaseMonoBehaviour
{
    [SerializeField]
    private GameObject pickupObj;

    [SerializeField]
    private Image pickupIcon;

    private Transform cameraTransform;

    private void OnEnable()
    {
        EventManager.StartListening(Events.HitShieldPickup, SetShieldIcon);
        EventManager.StartListening(Events.HitWeaponPickup, SetWeaponIcon);

        EventManager.StartListening(Events.ExitItemPickup, TurnOffPickup);
    }

    private void OnDisable()
    {
        EventManager.StopListening(Events.HitShieldPickup, SetShieldIcon);
        EventManager.StopListening(Events.HitWeaponPickup, SetWeaponIcon);

        EventManager.StopListening(Events.ExitItemPickup, TurnOffPickup);
    }

    private void Start()
    {
        cameraTransform = Camera.main.transform;
        pickupObj.SetActive(false);
    }

    public override void UpdateNormal()
    {
        RotateToFaceCamera();
    }

    private void RotateToFaceCamera()
    {
        this.transform.LookAt(new Vector3(cameraTransform.position.x,
                              this.transform.position.y,
                              cameraTransform.position.z));
    }

    private void TurnOnPickUp()
    {
        pickupObj.SetActive(true);
    }

    private void TurnOffPickup()
    {
        pickupObj.SetActive(false);
    }

    private void SetShieldIcon()
    {
        pickupIcon.sprite = StatePatternPlayableCharacter.Shield.ShieldIcon;

        TurnOnPickUp();
    }

    private void SetWeaponIcon()
    {
        pickupIcon.sprite = StatePatternPlayableCharacter.WeaponPickUp.Weapon.WeaponIcon;

        TurnOnPickUp();
    }


}
