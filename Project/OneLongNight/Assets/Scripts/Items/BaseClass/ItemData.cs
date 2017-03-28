using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/Item", order = 1)]
public class ItemData : ScriptableObject
{
    public ItemType itemType;
    public Sprite itemIcon;

    [Header("If Weapon")]
    public WeaponWeight weaponWeight;
    public WeaponType weaponType;
    public int weaponIndex;
    public float[] weaponAttackDamage = new float[4];
    public float[] weaponAttackCosts = new float[4];
    public int weaponDurability; //Defines how many attacks a weapon can make before it breaks;
    public float criticalHitDamage; //Defines how much damage 
    public float weaponRange; //How close the player has to get to use the weapon

    [Header("If Shield")]
    public int shieldIndex;
    public int shieldDurability; //Defines how many hits a shield can take before it breaks;

    [Header("Animations")]
    public MovementAnimations movementAnimations;
    public WeaponAnimations weaponAnimations;

    public enum ItemType
    {
        Weapon,
        Shield,
        Consumable
    }

    public enum WeaponType
    {
        OneHanded,
        TwoHanded,
    }

    public enum WeaponWeight
    {
        Light,
        Medium,
        Heavy,
    }

}
