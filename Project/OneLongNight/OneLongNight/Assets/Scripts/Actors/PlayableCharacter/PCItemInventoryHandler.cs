using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCItemInventoryHandler : BaseMonoBehaviour
{
    public static ItemPickup foundItem;
    private StatePatternPlayableCharacter player;
    private static ItemData item;
    public static ItemData Item
    {
        get
        {
            return item;
        }
    }

    [SerializeField]
    private ItemData startingWeapon;
    [SerializeField]
    private GameObject daggerPlaceholder;

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
    [SerializeField]
    private GameObject[] weapons;
    private int currentWeaponIndex;
    private bool hasWeapon;
    private static float weaponDurability;
    public static float WeaponDurability
    {
        get
        {
            return weaponDurability;
        }
    }
    private ItemData.WeaponType currentWeaponType;


    private void Start()
    {
        player = this.GetComponent<StatePatternPlayableCharacter>();
        SetStartingWeapon();

    }

    void SetStartingWeapon()
    {
        item = startingWeapon;
        currentWeaponType = startingWeapon.weaponType;
        weapons[5].SetActive(true);
        daggerPlaceholder.SetActive(false);
        hasWeapon = true;

        EventManager.TriggerEvent(Events.NewWeaponPickup);
    }

    public void PickUpItem()
    {
        item = foundItem.Item;

        //First - Determine what type of weapon it is

        switch(item.itemType)
        {
            case ItemData.ItemType.Weapon:
                PickUpWeapon();
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
        if(hasWeapon)
        {
            //Check if it's two handed
            if (item.weaponType == ItemData.WeaponType.TwoHanded)
            {
                if(player.HasShield)
                {
                    //If it is then drop the shield
                    //Turn off Current Shield
                    shields[currentShieldIndex].SetActive(false);

                    //Drop Current Shield
                    GameObject droppedShield = ItemManager.Instance.GetShield(currentShieldIndex);

                    droppedShield.transform.position = new Vector3(this.transform.position.x - 2.0f,
                                                                   1.0f,
                                                                   this.transform.position.z - 2.0f);
                    droppedShield.SetActive(true);

                    //Set Old Shield Durability
                    droppedShield.GetComponent<ItemPickup>().ItemDurability = shieldDurability;

                    player.HasShield = false;
                }
            }

            //Check if weapon is dagger
            if(weapons[5].activeInHierarchy)
            {
                weapons[5].SetActive(false);
                daggerPlaceholder.SetActive(true);
            }
            else
            {
                //Drop Current Weapon
                weapons[currentWeaponIndex].SetActive(false);

                GameObject droppedWeapon = ItemManager.Instance.GetWeapon(currentWeaponIndex);

                droppedWeapon.transform.position = new Vector3(this.transform.position.x,
                                                               1.0f,
                                                               this.transform.position.z);

                droppedWeapon.SetActive(true);

                //Set Old Weapon Durability
                droppedWeapon.GetComponent<ItemPickup>().ItemDurability = weaponDurability;
            }
        }

        //Set New Weapon Model
        currentWeaponIndex = item.weaponIndex;
        weapons[currentWeaponIndex].SetActive(true);

        //Set New Weapon's Durability
        weaponDurability = foundItem.ItemDurability;

        //Set New Weapon Icon
        EventManager.TriggerEvent(Events.NewWeaponPickup);

        currentWeaponType = item.weaponType;

        hasWeapon = true;
        foundItem.GetItem();
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
        else
        {
            //Check if we have two handed weapon
            if(currentWeaponType == ItemData.WeaponType.TwoHanded)
            {
                //Drop two handed weapon                
                weapons[currentWeaponIndex].SetActive(false);

                GameObject droppedWeapon = ItemManager.Instance.GetWeapon(currentWeaponIndex);

                droppedWeapon.transform.position = new Vector3(this.transform.position.x,
                                                               1.0f,
                                                               this.transform.position.z);

                droppedWeapon.SetActive(true);

                //Activate Dagger
                SetStartingWeapon();
            }
        }

        currentShieldIndex = item.shieldIndex;
        shields[currentShieldIndex].SetActive(true);
        player.HasShield = true;

        //Set Shield Durability
        shieldDurability = foundItem.ItemDurability;

        //Trigger UI Change
        EventManager.TriggerEvent(Events.NewShieldPickup);
        
        foundItem.GetItem();

        //Implement New Movement Animations

        //Implement New Combat Animations        
    }
}
