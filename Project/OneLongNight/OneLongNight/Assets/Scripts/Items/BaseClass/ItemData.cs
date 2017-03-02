using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/Item", order = 1)]
public class ItemData : ScriptableObject
{
    public ItemType itemType;
    public Sprite itemIcon;
    public GameObject itemPrefab;

    [Header("If Weapon")]
    public WeaponType weaponType;

    [Header("If Shield")]
    public int shieldIndex;

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

}
