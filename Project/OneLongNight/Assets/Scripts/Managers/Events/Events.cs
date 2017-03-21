using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Events
{
    public static string ReturnToIdle = "ReturnToIdle";

    //Item Pickup
    public static string HitItemPickup = "HitItemPickup"; // Triggers when player runs into an item pickup
    public static string ExitItemPickup = "ExitPickup"; //Triggers if Player Exits Weapon Pickup   
    public static string PickUpHealthPotion = "HealthPotionPickup"; //Triggered when the player picks up a health potion

    //Shield Pickup
    public static string NewShieldPickup = "NewShieldPickup;"; //Triggered when player picks up a new Shield

    //Weapon Pickup
    public static string NewWeaponPickup = "NewWeaponPickup"; //Triggered when player picks up a new Weapon

    //Combat===========================
    public static string HitEnemy = "HitEnemy"; //Triggered when the player hits an enemy with a weapon
    public static string UpdateWeaponDurability = "UpdateWeaponDurability"; //Triggered when the player's weapon hits an enemy;

    public static string PlayerStaggered = "PlayerStaggered"; //Triggered when the player is hit by the enemy but doesn't die.
    public static string PlayerDied = "PlayerDied"; //Triggered when the player dies

    //UI
    public static string HitByEnemy = "HitByEnemy"; //Triggered when the player is hit by an enemy's wapon
    public static string RegeneratingHealth = "RegenHealth"; //Triggered when the player is in a health zone

    

}
