using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Enemy"))
        {
            Debug.Log("Found Enemy");
            EventManager.TriggerEvent(Events.HitEnemy);
        }
    }
}
