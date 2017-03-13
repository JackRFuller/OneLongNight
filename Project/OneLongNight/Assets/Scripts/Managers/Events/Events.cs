using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Events
{
    public static string ReturnToIdle = "ReturnToIdle";

    //Item Pickup
    public static string HitItemPickup = "HitItemPickup"; // Triggers when player runs into an item pickup
    public static string ExitItemPickup = "ExitPickup"; //Triggers if Player Exits Weapon Pickup   

    //Shield Pickup
    public static string NewShieldPickup = "NewShieldPickup;"; //Triggered when player picks up a new Shield

    //Weapon Pickup
    public static string NewWeaponPickup = "NewWeaponPickup"; //Triggered when player picks up a new Weapon

    //Combat===========================
    public static string HitEnemy = "HitEnemy"; //Triggered when the player hits an enemy with a weapon
    public static string UpdateWeaponDurability = "UpdateWeaponDurability"; //Triggered when the player's weapon hits an enemy;
    

}
