using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCItemPickupHandler : BaseMonoBehaviour
{
    private StatePatternPlayableCharacter player;
    private ItemData item;

    [Header("Shields")]
    [SerializeField]
    private GameObject[] shields;
    private int currentShieldIndex;

    [Header("Weapons")]
    private GameObject[] weapons;

    private void Start()
    {
        player = this.GetComponent<StatePatternPlayableCharacter>();
    }

    public void PickUpItem(ItemData newItem)
    {
        item = newItem;

        //First - Determine what type of weapon it is

        switch(item.itemType)
        {
            case ItemData.ItemType.Weapon:
               
                break;

            case ItemData.ItemType.Shield:
                PickUpShield();
                break;

            case ItemData.ItemType.Consumable:
                break;
        }

    }

    //If Item is a Weapon
    private void PickUpWeapon()
    {
        //Check if it's two handed

        //If it is then drop the shield

        //Drop Current Weapon

        //Set New Weapon Model

        //Set New Weapon Icon

        //Set New Weapon's Health

        //Implement New Movement Animations

        //Implement New Combat Animations        
    }

    private void PickUpShield()
    {
        //Check if we already have a shield
        if(player.HasShield)
        {
            //Turn off Current Shield
            shields[currentShieldIndex].SetActive(false);

            //Get New Shield
            GameObject droppedShield = ItemManager.Instance.GetShield(currentShieldIndex);

            droppedShield.transform.position = new Vector3(this.transform.position.x,
                                                           1.0f,
                                                           this.transform.position.z);
            droppedShield.SetActive(true);
        }

        currentShieldIndex = item.shieldIndex;
        shields[currentShieldIndex].SetActive(true);
        player.HasShield = true;

        StatePatternPlayableCharacter.item.GetItem();
    }
}
