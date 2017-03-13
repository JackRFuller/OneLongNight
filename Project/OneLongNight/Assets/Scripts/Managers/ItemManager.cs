using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoSingleton<ItemManager>
{
    [Header("Shields")]
    [SerializeField]
    private GameObject[] shieldPickupPrefabs;
    private List<GameObject> shieldPickups;
    private const int numberOfShieldPickups = 5;

    [Header("Weapons")]
    [SerializeField]
    private GameObject[] weaponPickupPrefabs;
    private List<GameObject> weaponPickups;
    private const int numberOfWeaponPickups = 5;

    private void Start()
    {
        SpawnInShieldPickups();
        SpawnInWeaponPickups();
    }

    private void SpawnInWeaponPickups()
    {
        weaponPickups = new List<GameObject>();

        GameObject weaponHolder = new GameObject();
        weaponHolder.name = "WeaponHolder";

        for(int i = 0; i < weaponPickupPrefabs.Length; i++)
        {
            for(int j = 0; j < numberOfWeaponPickups; j++)
            {
                GameObject weapon = Instantiate(weaponPickupPrefabs[i]) as GameObject;

                weaponPickups.Add(weapon);

                weapon.transform.parent = weaponHolder.transform;
                weapon.transform.localPosition = Vector3.zero;
                weapon.SetActive(false);
            }
        }
    }

    public GameObject GetWeapon(int weaponIndex)
    {
        for(int i = 0; i < weaponPickups.Count; i++)
        {
            if(weaponPickups[i].GetComponent<ItemPickup>().Item.weaponIndex == weaponIndex)
            {
                if(!weaponPickups[i].activeInHierarchy)
                {
                    return weaponPickups[i];
                }
            }
        }

        Debug.LogError("NO WEAPON FOUND!!!!");
        return new GameObject();
    }

    private void SpawnInShieldPickups()
    {
        shieldPickups = new List<GameObject>();

        GameObject shieldHolder = new GameObject();
        shieldHolder.name = "ShieldHolder";

        for (int i = 0; i < shieldPickupPrefabs.Length; i++)
        {
            for (int j = 0; j < numberOfShieldPickups; j++)
            {
                GameObject shield = Instantiate(shieldPickupPrefabs[i]) as GameObject;

                shieldPickups.Add(shield);

                shield.transform.parent = shieldHolder.transform;
                shield.transform.localPosition = Vector3.zero;
                shield.SetActive(false);
            }
        }  
    }

    public GameObject GetShield(int shieldType)
    {
        for(int i = 0; i < shieldPickups.Count; i++)
        {
            if(shieldPickups[i].GetComponent<ItemPickup>().Item.shieldIndex == shieldType)
            {
                if(!shieldPickups[i].activeInHierarchy)
                    return shieldPickups[i];
            }
        }
        Debug.LogError("No Shield Found!!!!");
        return new GameObject();
    }
}
