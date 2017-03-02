using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Events
{
    public static string ReturnToIdle = "ReturnToIdle";

    //Item Pickup
    public static string HitItemPickup = "HitItemPickup"; // Triggers when player runs into an item pickup

    public static string ChnagedWeapon = "ChangedWeapon"; //Triggers when the player picks up a weapon
    public static string HitShieldPickup = "HitShieldPickup"; //Triggers if player runs into shield pickup
    public static string HitWeaponPickup = "HitWeaponPickup"; //Triggers if Player Runs into Weapon Pickup;
    public static string ExitItemPickup = "ExitPickup"; //Triggers if Player Exits Weapon Pickup

}
