using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCItemInventoryHandler : BaseMonoBehaviour
{
    private StatePatternPlayableCharacter player;
    private ItemData item;

    [Header("Shields")]
    [SerializeField]
    private GameObject[] shields;
    private int currentShieldIndex;
    private static float shieldDurability;
    public static float ShieldDurability
    {
        get
        {
            return shieldDurability;
        }
    }

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
        //Check if we already have a weapon


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

            //Drop Current Shield
            GameObject droppedShield = ItemManager.Instance.GetShield(currentShieldIndex);

            droppedShield.transform.position = new Vector3(this.transform.position.x,
                                                           1.0f,
                                                           this.transform.position.z);
            droppedShield.SetActive(true);

            //Set Old Shield Durability
            droppedShield.GetComponent<ItemPickup>().ItemDurability = shieldDurability; 
        }

        currentShieldIndex = item.shieldIndex;
        shields[currentShieldIndex].SetActive(true);
        player.HasShield = true;

        //Set Shield Durability
        shieldDurability = StatePatternPlayableCharacter.item.ItemDurability;

        //Trigger UI Change
        EventManager.TriggerEvent(Events.NewShieldPickup);

        
        StatePatternPlayableCharacter.item.GetItem();
    }
}
