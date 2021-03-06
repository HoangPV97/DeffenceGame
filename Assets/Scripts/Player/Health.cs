﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Health
{
    [SerializeField]
    private int mHealth;
    [SerializeField]
    private int currentHealth;
    [SerializeField]
    private int recoverHealthValue;
    [SerializeField]
    private float recoverHealthTime;
    public Image healthBar;
    public Image effecthealthBar;
    public TextMeshProUGUI healthValueText;
    public int health
    {
        get
        {
            return this.mHealth;
        }
        set
        {
            this.mHealth = value;
        }
    }
    public int CurrentHealth
    {
        get
        {
            return this.currentHealth;
        }
        set
        {
            this.currentHealth = value;
        }
    }
    public int RecoverHealthValue
    {
        get
        {
            return this.recoverHealthValue;
        }
        set
        {
            this.recoverHealthValue = value;
        }
    }
    public float RecoverHealthTime
    {
        get
        {
            return this.recoverHealthTime;
        }
        set
        {
            this.recoverHealthTime = value;
        }
    }
    public void Init(int Hp, int HpRegen)
    {
        health = Hp;
        CurrentHealth = health;
        recoverHealthValue = HpRegen;
        UpdateValueText();
        healthBar.fillAmount = 1;
    }
    public void ReduceHealth(int _damage)
    {
        CurrentHealth -= _damage;
        if (CurrentHealth < 0)
        {
            CurrentHealth = 0;
        }
        healthBar.fillAmount = (float)CurrentHealth / health * 1.0f;
        UpdateValueText();
    }
    public void EffectHealth()
    {
        effecthealthBar.fillAmount = healthBar.fillAmount;
    }
    public void RecoverHealth()
    {
        CurrentHealth += RecoverHealthValue;
        healthBar.fillAmount += (float)RecoverHealthValue / health;
        if (CurrentHealth > health)
        {
            CurrentHealth = health;
        }
        UpdateValueText();
    }
    public void IncreaseRecoverHealthValue(int _Value)
    {
        recoverHealthValue += recoverHealthValue * _Value / 100;
    }
    public void UpdateValueText()
    {
        if (healthValueText != null)
        {
            healthValueText.text = currentHealth.ToString() + "/" + health;
        }
    }
}
