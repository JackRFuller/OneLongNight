using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthZoneHandler : BaseMonoBehaviour
{
    [SerializeField]
    private float enabledTime;
    [SerializeField]
    private float healthRegenAmount;

    public override void UpdateNormal()
    {
        RunExisitngTimer();
    }

    private void RunExisitngTimer()
    {
        enabledTime -= Time.deltaTime;
        if(enabledTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag.Equals("Player"))
        {
            if(PCAttributes.Health < 100.0f)
            {
                PCAttributes.Instance.HeathRegenerate(healthRegenAmount);
            }
            
        }
    }



}
