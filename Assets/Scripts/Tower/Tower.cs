using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Tower : MonoBehaviour
{
    public Health Health;
    public Mana Mana;
    public static bool Islive;
    public int ShieldBlockValue;
    public int ShieldBlockChance;
    public GameObject TowerEffect;
    bool StartRecoverHealth, StarRecoverMana;
    private void Start()
    {
        Islive = true;
    }
    public void TakeDamage(int _damage)
    {
        int BlockChance = Random.Range(0, 100);
        _damage -= ShieldBlockValue;
        if (_damage < 0 || BlockChance<ShieldBlockChance)
            _damage = 0;
        TowerEffect.SetActive(true);
        Health.ReduceHealth((_damage));
        if (!StartRecoverHealth)
        {
            InvokeRepeating("RecoverHealth", 0, Mana.RecoverManaTime);
            StartRecoverHealth = true;
        }
        StartCoroutine(WaitingEffectHealth());
        if (Health.CurrentHealth <= 0)
        {
            Die();
        }
    }
    public void SetUpData()
    {
        var BaseData= DataController.Instance.InGameBaseData;
        Health.Init((int)BaseData.HP,(int)BaseData.HPRegen);
        Mana.Init(BaseData.Mana, BaseData.ManaRegen);
        ShieldBlockValue = (int)BaseData.ShieldBlockValue;
        ShieldBlockChance = (int)BaseData.ShieldBlockChance;
    }

    public void ReduceMana(float _mana)
    {
        Mana.ConsumeMana(_mana);
        if (!StarRecoverMana)
        {
            InvokeRepeating("RecoverMana", 0, Mana.RecoverManaTime);
            StarRecoverMana = true;
        }
    }

    IEnumerator WaitingRecoverMana()
    {
        yield return new WaitForSeconds(Mana.RecoverManaTime);

    }
    private void Die()
    {
        Islive = false;
        GameController.Instance.EndGame();
    }
    public void RecoverMana()
    {
        if (Mana.CurrentMana < Mana.maxMana)
        {
            Mana.RecoverMana();
        }
        else
        {
            StarRecoverMana = false;
            StopRecoverMana();
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
        else if(Health.CurrentHealth > Health.health || Health.CurrentHealth<=0)
        {
            StarRecoverMana = false;
            StopRecoverHealth();
        }
    }
    public void StopRecoverHealth()
    {
        CancelInvoke("RecoverHealth");
    }
    public void StopRecoverMana()
    {
        CancelInvoke("RecoverMana");
    }
}
