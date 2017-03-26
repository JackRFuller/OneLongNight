using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCAttackAnimationController : StateMachineBehaviour
{
    public int attackIndex;

    private void OnEnable()
    {
        PCItemInventoryHandler.Instance.AddPCAttackController(this, attackIndex);
    }

}
