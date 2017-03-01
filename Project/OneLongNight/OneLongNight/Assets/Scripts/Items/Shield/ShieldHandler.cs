using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldHandler : MonoBehaviour
{
    [SerializeField]
    private Sprite shieldIcon;
    public Sprite ShieldIcon
    {
        get
        {
            return shieldIcon;
        }
    }

    [SerializeField]
    private Collider shieldCollider;

    [SerializeField]
    private MeshRenderer shieldMesh;

    public void PickUpItem()
    {
        shieldCollider.enabled = false;
        shieldMesh.enabled = false;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Player"))
        {
            StatePatternPlayableCharacter.Shield = this;
            EventManager.TriggerEvent(Events.HitShieldPickup);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {            
            StatePatternPlayableCharacter.Shield = null;
            EventManager.TriggerEvent(Events.ExitItemPickup);
        }
    }
}
