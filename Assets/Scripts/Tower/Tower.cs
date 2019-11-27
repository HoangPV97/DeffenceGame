using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public Health Health;
    public Mana Mana;
    public static bool Islive;
    private void Start()
    {
        Islive = true;
        Mana.Init();
        Health.Init();
        InvokeRepeating("RecoverMana",0, Mana.RecoverManaTime);
        InvokeRepeating("RecoverHealth", 0, Mana.RecoverManaTime);
    }
    public void TakeDamage(float _damage)
    {
        Health.ReduceHealth(_damage);
            if (Health.CurrentHealth <= 0)
            {
                Die();
            }
    }
    public void ReduceMana(float _mana)
    {
        Mana.ConsumeMana(_mana);
        
    }

    IEnumerator WaitingRecoverMana()
    {
        yield return new WaitForSeconds(Mana.RecoverManaTime);
        
    }
    private void Die()
    {
        Islive = false;
    }
    public void RecoverMana()
    {
        if (Mana.CurrentMana < Mana.maxMana)
        {
            Mana.RecoverMana();
        }
    }
    public void RecoverHealth()
    {
        if (Health.CurrentHealth < Health.health)
        {
            Health.RecoverHealth();
        }
    }
}
