using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : BaseMonoBehaviour
{
    [SerializeField]
    private Transform itemModel;
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
    private float itemDurability;
    public float ItemDurability
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
        itemModelStartPos = itemModel.transform.localPosition;
    }

    public void GetItem()
    {
        //Reset Item Model Position
        itemModel.transform.localPosition = itemModelStartPos;

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
        yield return new WaitForSeconds(2f);
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
