using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoSingleton<TimeManager>
{
    private float combatSlowMoTimer = 0.1f;
    private bool isRunningTimer;

    private void OnEnable()
    {
        EventManager.StartListening(Events.HitEnemy, SetSlowMoForCombatHit);
    }

    private void OnDisable()
    {
        EventManager.StopListening(Events.HitEnemy, SetSlowMoForCombatHit);
    }

    private void SetSlowMoForCombatHit()
    {
        if(!isRunningTimer)
        {
            Time.timeScale = 0.3f;
            isRunningTimer = true;
        }
       
    }

    private void Update()
    {
        if (isRunningTimer)
        {
            combatSlowMoTimer -= Time.deltaTime;
            //Debug.Log(combatSlowMoTimer);
        }
        if(combatSlowMoTimer < 0)        
        {
            combatSlowMoTimer = 0.2f;
            Time.timeScale = 1.0f;
            isRunningTimer = false;
        }
    }

}
