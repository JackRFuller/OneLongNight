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

    private IEnumerator cooldownRoutine;

    private bool isRegeneratingStamina;

    [Header("Stamina Attributes")]
    [SerializeField]
    private float staminaRegenRate = 10.0f;

    private void Start()
    {
        pcStamina = startingPCStamina;
        cooldownRoutine = CooldownToStartStaminaRegneration();
    }

    public void RemoveStamina(float amount)
    {
        pcStamina -= amount;
        isRegeneratingStamina = false;

        StopCoroutine(cooldownRoutine);
        cooldownRoutine = CooldownToStartStaminaRegneration();
        StartCoroutine(cooldownRoutine);
    }

    public bool CheckIfPCHasEnoughStamina(float cost)
    {
        if (pcStamina >= cost)
            return true;
        else
            return false;
    }


    IEnumerator CooldownToStartStaminaRegneration()
    {
        yield return new WaitForSeconds(5.0f);
        isRegeneratingStamina = true;
    }

    private void Update()
    {
        if (isRegeneratingStamina)
            RegenerateStamina();
    }

    private void RegenerateStamina()
    {
        pcStamina += staminaRegenRate * Time.deltaTime;
        if(pcStamina >= 100.0f)
        {
            isRegeneratingStamina = false;
        }
    }
}
