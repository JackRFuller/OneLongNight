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

    private void Start()
    {
        SpawnInShieldPickups();
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
