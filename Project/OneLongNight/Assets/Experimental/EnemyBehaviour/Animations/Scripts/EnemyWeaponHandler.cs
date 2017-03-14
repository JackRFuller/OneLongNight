using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponHandler : BaseMonoBehaviour
{
    public float weaponDamage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            other.SendMessage("HitByEnemy",weaponDamage, SendMessageOptions.DontRequireReceiver);
        }
    }
}
