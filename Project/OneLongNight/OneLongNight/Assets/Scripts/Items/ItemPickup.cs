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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            StatePatternPlayableCharacter.item = this;
            EventManager.TriggerEvent(Events.HitItemPickup);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            StatePatternPlayableCharacter.item = null;
            EventManager.TriggerEvent(Events.ExitItemPickup);
        }
    }
}
