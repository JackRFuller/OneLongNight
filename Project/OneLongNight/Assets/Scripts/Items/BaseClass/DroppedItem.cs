using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DroppedItem
{
    public ItemType itemType;
    [Range(0, 100)]
    public int chanceOfDrop;

    public enum ItemType
    {
        Shield,
        Staff,
        Sword,
        GreatAxe,
        Scythe,
        Axe,
        HealthPotion
    }

}
