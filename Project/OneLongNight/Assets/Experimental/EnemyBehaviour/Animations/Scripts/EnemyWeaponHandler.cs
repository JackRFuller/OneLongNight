﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponHandler : BaseMonoBehaviour
{
    private AudioSource audio;
    [HideInInspector] public Transform wielder;
    [HideInInspector] public float weaponDamage;

    private void Start()
    {
        audio = this.GetComponent<AudioSource>();
        this.GetComponent<Collider>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            Vector3 pointOfContact = Vector3.zero;
            RaycastHit hit;
            audio.PlayOneShot(audio.clip);

            if (Physics.Raycast(transform.position, transform.forward, out hit))
            {
                pointOfContact = hit.point;
            }

            HitInfo hitInfo = new HitInfo();
            hitInfo.attacker = wielder;
            hitInfo.hitDirection = pointOfContact;
            hitInfo.damage = weaponDamage;

            other.SendMessage("HitByEnemy",hitInfo, SendMessageOptions.DontRequireReceiver);
        }
    }
}
