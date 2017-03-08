using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCItemInventoryHandler : BaseMonoBehaviour
{
    public static ItemPickup foundItem;
    public static bool PickUpFinished = true;

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
    public static ItemData CurrentWeapon;

    private void Start()
    {
        player = this.GetComponent<StatePatternPlayableCharacter>();
        SetStartingWeapon();
        OverrideAnimationClips();

    }

    void SetStartingWeapon()
    {
        item = startingWeapon;
        currentWeaponType = startingWeapon.weaponType;
        CurrentWeapon = startingWeapon;
        weapons[5].SetActive(true);
        daggerPlaceholder.SetActive(false);
        hasWeapon = true;

        EventManager.TriggerEvent(Events.NewWeaponPickup);
    }

    public void PickUpItem()
    {
        PickUpFinished = false;
        item = foundItem.Item;

        switch (item.itemType)
        {
            case ItemData.ItemType.Weapon:
                StartCoroutine(PickUpWeapon());
                break;

            case ItemData.ItemType.Shield:
                StartCoroutine(PickUpShield());
                break;

            case ItemData.ItemType.Consumable:
                break;
        }
    }

    

    //If Item is a Weapon
    IEnumerator PickUpWeapon()
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

                    droppedShield.transform.position = new Vector3(this.transform.position.x - 1.75f,
                                                                   1.0f,
                                                                   this.transform.position.z - 1.75f);
                    droppedShield.SetActive(true);

                    //Set Old Shield Durability
                    ItemPickup droppedItem = droppedShield.GetComponent<ItemPickup>();
                    droppedItem.ItemDurability = shieldDurability;
                    droppedItem.ReactivateItem();

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
                ItemPickup droppedItem = droppedWeapon.GetComponent<ItemPickup>();
                droppedItem.ItemDurability = weaponDurability;
                droppedItem.ReactivateItem();
                
            }
        }

        //Used to line up Pickup with Animation
        yield return new WaitForSeconds(0.9f);

        //Set New Weapon Model
        currentWeaponIndex = item.weaponIndex;
        weapons[currentWeaponIndex].SetActive(true);

        //Set New Weapon's Durability
        weaponDurability = foundItem.ItemDurability;

        //Set New Weapon Icon
        EventManager.TriggerEvent(Events.NewWeaponPickup);

        currentWeaponType = item.weaponType;
        CurrentWeapon = item;

        hasWeapon = true;
        foundItem.GetItem();

        OverrideAnimationClips();
    }

    IEnumerator PickUpShield()
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
            ItemPickup droppedItem = droppedShield.GetComponent<ItemPickup>();
            droppedItem.ItemDurability = shieldDurability;
            droppedItem.ReactivateItem();
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

                //Set Old Weapon Durability
                ItemPickup droppedItem = droppedWeapon.GetComponent<ItemPickup>();
                droppedItem.ItemDurability = weaponDurability;
                droppedItem.ReactivateItem();

                //Activate Dagger
                SetStartingWeapon();
            }
        }
        //Used to line up Pickup with Animation
        yield return new WaitForSeconds(0.9f);

        currentShieldIndex = item.shieldIndex;
        shields[currentShieldIndex].SetActive(true);
        player.HasShield = true;

        //Set Shield Durability
        if(foundItem)
            shieldDurability = foundItem.ItemDurability;

        //Trigger UI Change
        EventManager.TriggerEvent(Events.NewShieldPickup);
        
        foundItem.GetItem();

        OverrideAnimationClips();       
    }

    //Takes New Weapon and Sets Up New Animations
    private void OverrideAnimationClips()
    {
        Animator animator = GetComponent<Animator>();

        AnimatorOverrideController overrideController = new AnimatorOverrideController();
        overrideController.runtimeAnimatorController = GetEffectiveController(animator);

        //Set Up New Animation Clips - Movement   

        overrideController["Idle"] = item.movementAnimations.idleAnim.clip;        

        overrideController["Move"] = item.movementAnimations.moveAnim.clip;

        overrideController["Roll"] = item.movementAnimations.rollAnim.clip;

        overrideController["BlockMove"] = item.movementAnimations.blockingMoveAnim.clip;

        overrideController["BlockIdle"] = item.movementAnimations.blockingIdle.clip;
       

        animator.runtimeAnimatorController = overrideController;

        PickUpFinished = true;
    }

    //Create New Animator
    private RuntimeAnimatorController GetEffectiveController(Animator animator)
    {
        RuntimeAnimatorController controller = animator.runtimeAnimatorController;

        AnimatorOverrideController overrideController = controller as AnimatorOverrideController;
        while (overrideController != null)
        {
            controller = overrideController.runtimeAnimatorController;
            overrideController = controller as AnimatorOverrideController;
        }

        return controller;
    }
}
