using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : BaseMonoBehaviour
{
    [SerializeField]
    private WeaponData weapon;
    public WeaponData Weapon
    {
        get
        {
            return weapon;
        }
    }

    [SerializeField]
    private int weaponIndex;
    public int WeaponIndex
    {
        get
        {
            return weaponIndex;
        }
    }

    [SerializeField]
    private Transform weaponSpawnPoint;
    private GameObject weaponObject;
    private Mesh weaponMesh;
    public Mesh WeaponMesh
    {
        get
        {
            return weaponMesh;
        }
    }

    private void Start()
    {
        weaponObject = Instantiate(weapon.WeaponPrefab) as GameObject;

        weaponObject.transform.parent = this.transform;

        weaponObject.transform.position = weaponSpawnPoint.position;
        weaponObject.transform.rotation = weaponSpawnPoint.rotation;

        weaponMesh = weaponObject.GetComponent<MeshFilter>().mesh;
    }

    /// <summary>
    /// Called from StatePatternPlayable when Player Presses E
    /// </summary>
    public void GetWeapon()
    {
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Player"))
        {
            StatePatternPlayableCharacter.WeaponPickUp = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag.Equals("Player"))
        {
            StatePatternPlayableCharacter.WeaponPickUp = null;
        }
    }

}
