using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCAttributes : MonoSingleton<PCAttributes>
{
    

    private const float startingHealth = 100.0f;
    private static float health;
    public static float Health
    {
        get
        {
            return health;
        }  
        set
        {
            health = value;
        }      
    }

    private const float startingPCStamina = 100000.0f;

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
        health = startingHealth;

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


    //Triggered when the player is inside a healing zone
    public void HeathRegenerate(float _amount)
    {
        health += _amount * Time.deltaTime;
        EventManager.TriggerEvent(Events.RegeneratingHealth);
        if(health > 100)
        {
            health = 100.0f;
        }
    }

    /// <summary>
    /// Used to make the PC invulnerable during rolling
    /// </summary>
    public void TurnOnCollider()
    {
        this.gameObject.layer = LayerMask.NameToLayer("Player");
    }

    public void TurnOffCollider()
    {
        this.gameObject.layer = LayerMask.NameToLayer("Invulnerable");
    }
}
