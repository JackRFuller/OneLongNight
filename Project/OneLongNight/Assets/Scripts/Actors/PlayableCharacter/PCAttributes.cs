using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCAttributes : MonoSingleton<PCAttributes>
{
    private const float startingPCStamina = 100.0f;

    private float pcStamina;
    public float PCStamina
    {
        get
        {
            return pcStamina;
        }
    }

    private void Start()
    {
        pcStamina = startingPCStamina;
    }

    public bool CheckIfPCHasEnoughStamina(float cost)
    {
        if (pcStamina >= cost)
            return true;
        else
            return false;
    }
}
