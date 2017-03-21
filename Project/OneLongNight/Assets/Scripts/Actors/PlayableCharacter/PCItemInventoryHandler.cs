using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCItemInventoryHandler : MonoSingleton<PCItemInventoryHandler>
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
    private static int shieldDurability;
    public static int ShieldDurability
    {
        get
        {
            return shieldDurability;
        }
    }
    private bool justPickedUpShield;
    public static ItemData CurrentShield;

    //Consumables
    private static int healthPotionCount;
    public static int HealthPotionCount
    {
        get
        {
            return healthPotionCount;
        }
    }

    [Header("Weapons")]
    [SerializeField]
    private GameObject[] weapons;    
    private List<WeaponHandler> weaponHandlers;
    public static WeaponHandler currentWeaponHandler; //Holds the script for the current weapon handler
    private int currentWeaponIndex;
    private bool hasWeapon;
    private static int weaponDurability;
    public static int WeaponDurability
    {
        get
        {
            return weaponDurability;
        }
    }
    private ItemData.WeaponType currentWeaponType;
    public static ItemData CurrentWeapon;

    private void OnEnable()
    {
        EventManager.StartListening(Events.UpdateWeaponDurability, UpdateWeaponDurability);
    }

    private void OnDisable()
    {
        EventManager.StopListening(Events.UpdateWeaponDurability, UpdateWeaponDurability);
    }

    private void Start()
    {
        player = this.GetComponent<StatePatternPlayableCharacter>();

        //Get Colliders to Turn On And Off       
        weaponHandlers = new List<WeaponHandler>();

        for(int i = 0; i < weapons.Length; i++)
        {           
            weaponHandlers.Add(weapons[i].GetComponent<WeaponHandler>());
        }

        SetoToDagger();      
    }

    /// <summary>
    /// Set Starting Weapon to Dagger
    /// </summary>
    void SetoToDagger()
    {
        item = startingWeapon;
        currentWeaponType = startingWeapon.weaponType;
        CurrentWeapon = startingWeapon;
        weapons[5].SetActive(true);
        daggerPlaceholder.SetActive(false);

        //Set Weapon Stats
        currentWeaponHandler = weaponHandlers[5];
        currentWeaponHandler.UpdateWeaponDamage(item.weaponAttackDamage);
        weaponDurability = item.weaponDurability;

        hasWeapon = true;

        EventManager.TriggerEvent(Events.NewWeaponPickup);

        OverrideAnimationClips();
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
        }
    }

    public void PickUpHealthPotion()
    {
        healthPotionCount++;
        EventManager.TriggerEvent(Events.PickUpHealthPotion);
    }

    public void UsedHealthPotion()
    {
        healthPotionCount--;
        EventManager.TriggerEvent(Events.PickUpHealthPotion);
    }

    public void ToggleWeapon()
    {
        currentWeaponHandler.ToggleWeapon();
    }

    public void UpdateWeaponDurability()
    {
        weaponDurability--;

        //Debug.Log(weaponDurability);        
        if(weaponDurability == 0)
        {
            //Turn Off Current Weapon
            weapons[currentWeaponIndex].SetActive(false);

            //Change To Dagger
            SetoToDagger();

            //TODO --- Impact Attack State
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
        yield return new WaitForSeconds(0.5f);

        //Set New Weapon Model
        currentWeaponIndex = item.weaponIndex;
        weapons[currentWeaponIndex].SetActive(true);

        //Set New Weapon's Durability
        weaponDurability = foundItem.ItemDurability;

        //Set Weapon Damage Out
        weaponHandlers[currentWeaponIndex].UpdateWeaponDamage(item.weaponAttackDamage);

        //Set New Weapon Icon
        EventManager.TriggerEvent(Events.NewWeaponPickup);

        currentWeaponType = item.weaponType;
        CurrentWeapon = item;
        currentWeaponHandler = weaponHandlers[currentWeaponIndex];

        hasWeapon = true;
        
        //Turn off Pickup
        foundItem.GetItem();

        //Update Animation
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
                SetoToDagger();
            }
        }
        //Used to line up Pickup with Animation
        yield return new WaitForSeconds(0.5f);

        currentShieldIndex = item.shieldIndex;
        shields[currentShieldIndex].SetActive(true);
        player.HasShield = true;

        //Set Shield Durability
        if(foundItem)
            shieldDurability = foundItem.ItemDurability;

        //Trigger UI Change
        EventManager.TriggerEvent(Events.NewShieldPickup);
        
        foundItem.GetItem();

        justPickedUpShield = true;
        CurrentShield = item;

        OverrideAnimationClips();       
    }

    //Takes New Weapon and Sets Up New Animations
    private void OverrideAnimationClips()
    {
        Animator animator = GetComponent<Animator>();

        AnimatorOverrideController overrideController = new AnimatorOverrideController();
        overrideController.runtimeAnimatorController = GetEffectiveController(animator);

        //If Player Picked Up a Shield

       //Conditions
       // 1 - If player picksup a new two handed weapon
       // 2 - If player picks up a shield
        
        //Set Up New Animation Clips - Movement
        overrideController["Idle"] = item.movementAnimations.idleAnim.clip;            

        overrideController["Move"] = item.movementAnimations.moveAnim.clip;

        overrideController["Roll"] = item.movementAnimations.rollAnim.clip;

        overrideController["BlockMove"] = item.movementAnimations.blockingMoveAnim.clip;

        overrideController["BlockIdle"] = item.movementAnimations.blockingIdle.clip;

        if (!justPickedUpShield)
        {
            //Set Up New Animation Clips - Combat
            overrideController["Attack1"] = item.weaponAnimations.attackOneAnim.clip;
            animator.SetFloat("Attack1Speed", item.weaponAnimations.attackOneAnim.clipSpeed);

            overrideController["Attack2"] = item.weaponAnimations.attackTwoAnim.clip;
            animator.SetFloat("Attack2Speed", item.weaponAnimations.attackTwoAnim.clipSpeed);

            overrideController["Attack3"] = item.weaponAnimations.attackThreeAnim.clip;
            animator.SetFloat("Attack3Speed", item.weaponAnimations.attackThreeAnim.clipSpeed);

            overrideController["Attack4"] = item.weaponAnimations.attackFourAnim.clip;
            animator.SetFloat("Attack4Speed", item.weaponAnimations.attackFourAnim.clipSpeed);

            animator.runtimeAnimatorController = overrideController;

            
        }

        justPickedUpShield = false;       

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
