using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEvents : BaseMonoBehaviour
{
    [SerializeField]
    private Collider enemyWeaponCollider;

    public void TurnOffWeaponCollider()
    {
        enemyWeaponCollider.enabled = false;
    }

    public void TurnOnWeaponCollider()
    {
        enemyWeaponCollider.enabled = true;
    }

}
