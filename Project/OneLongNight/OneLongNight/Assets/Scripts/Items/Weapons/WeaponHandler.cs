using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : BaseMonoBehaviour
{
    private Collider weaponCollider;

    private List<int> attackIndexes = new List<int>(); //Queues up List of Attack Indexes

    private float[] damageToInflict;
    private float criticalHitDamage;
       
    private int attackIndex;

    private Transform playerTransform;

    private void Start()
    {
        playerTransform = PCAttributes.Instance.transform;

        weaponCollider = this.GetComponent<Collider>();
        weaponCollider.enabled = false;
    }

    //Strarting a New Series of Attacks thus we should clear the list and make the first entry 0
    public void StartQueuingAttacks()
    {
        attackIndexes.Clear();
        attackIndexes.Add(0);

        attackIndex = -1; //Start AT -1 So the First Attack is Always 0
    }

    public void ToggleWeapon()
    {
        if(weaponCollider.enabled == false)
        {
            weaponCollider.enabled = true;
            //If We're Turning Weapon On We Increment The Attack Index
            attackIndex++;
        }
        else
        {
            weaponCollider.enabled = false;
        }
    }

    public void AddAttackIndexToQueue(int _index)
    {
        attackIndexes.Add(_index);
    }
    
    public void UpdateWeaponDamage(float[] _damageToInflict)
    {
        damageToInflict = _damageToInflict;
    }   
    
    public void UpdateAttackIndex(int _attackIndex)
    {
        attackIndex = _attackIndex;
    } 

    private void OnTriggerEnter(Collider other)
    {
        //Find Enemy
        if(other.tag.Equals("Enemy"))
        {
            ////Look At Enemy
            //Vector3 lookAtPos = new Vector3(other.transform.position.x,
            //                                playerTransform.position.y,
            //                                other.transform.position.z);

            //playerTransform.LookAt(lookAtPos);
                                            

            float _damageToInflict = 0;

            //Check if the weapon is about to break i.e Durability = 1
            if(PCItemInventoryHandler.WeaponDurability == 1)
            {
                //Implement Crit Hit Damage
            }
            else
            {
                _damageToInflict = damageToInflict[attackIndexes[attackIndex]];
            }

            //Send Damage
            other.SendMessage("HitByPlayer", _damageToInflict, SendMessageOptions.DontRequireReceiver);

            //Update Durability
            EventManager.TriggerEvent(Events.UpdateWeaponDurability);

            EventManager.TriggerEvent(Events.HitEnemy);

        }
    }
}
