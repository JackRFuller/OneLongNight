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
        
        //Used to line up Pickup with Animation
        yield return new WaitForSeconds(0.5f);

        
       
    }

    IEnumerator PickUpShield()
    {
        
        //Used to line up Pickup with Animation
        yield return new WaitForSeconds(0.5f);

        
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
