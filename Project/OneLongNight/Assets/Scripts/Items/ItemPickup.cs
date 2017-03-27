using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : BaseMonoBehaviour
{
   
    private Vector3 itemModelStartPos;

    [SerializeField]
    private ItemData item;
    public ItemData Item
    {
        get
        {
            return item;
        }
    }

    [SerializeField]
    private int itemDurability;
    public int ItemDurability
    {
        get
        {
            return itemDurability;
        }
        set
        {
            itemDurability = value;
        }
    }

    private void Start()
    {
        switch(item.itemType)
        {
            case ItemData.ItemType.Weapon:
                itemDurability = item.weaponDurability;
                break;

            case ItemData.ItemType.Shield:
                itemDurability = item.shieldDurability;
                break;
        }
    }

    public void GetItem()
    {
        EventManager.TriggerEvent(Events.ExitItemPickup);
        this.gameObject.SetActive(false);
        PCItemInventoryHandler.foundItem = null;
    }

    public void ReactivateItem()
    {
        this.GetComponent<Collider>().enabled = false;
        StartCoroutine(TurnOnCollider());
    }

    IEnumerator TurnOnCollider()
    {
        yield return new WaitForSeconds(0f);
        this.GetComponent<Collider>().enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            PCItemInventoryHandler.foundItem = this;
            EventManager.TriggerEvent(Events.HitItemPickup);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            PCItemInventoryHandler.foundItem = null;
            EventManager.TriggerEvent(Events.ExitItemPickup);
        }
    }
}
