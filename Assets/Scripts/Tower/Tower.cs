using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public Health Health;
    public Mana Mana;
    public static bool Islive;
    public GameObject TowerEffect;
    private void Start()
    {
        Islive = true;
        // Mana.Init();
        //  Health.Init();
        InvokeRepeating("RecoverMana", 0, Mana.RecoverManaTime);
        InvokeRepeating("RecoverHealth", 0, Mana.RecoverManaTime);
    }
    public void TakeDamage(float _damage)
    {
        TowerEffect.SetActive(true);
        Health.ReduceHealth(_damage);
        StartCoroutine(WaitingEffectHealth());
        if (Health.CurrentHealth <= 0)
        {
            Die();
        }
    }

    public void SetUpData()
    {
        Health.Init(DataController.Instance.InGameBaseData.HP, DataController.Instance.InGameBaseData.HPRegen);
        Mana.Init(DataController.Instance.InGameBaseData.Mana, DataController.Instance.InGameBaseData.ManaRegen);
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
    IEnumerator WaitingEffectHealth()
    {
        yield return new WaitForSeconds(0.5f);
        Health.EffectHealth();
    }
    public void RecoverHealth()
    {
        TowerEffect.SetActive(false);
        if (Health.CurrentHealth < Health.health)
        {
            Health.RecoverHealth();
        }
    }
}
