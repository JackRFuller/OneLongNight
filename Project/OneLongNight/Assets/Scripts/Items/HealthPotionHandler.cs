using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPotionHandler : BaseMonoBehaviour
{
    

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Player"))
        {
            PCItemInventoryHandler.Instance.PickUpHealthPotion();
            this.gameObject.SetActive(false);
        }
    }

}
