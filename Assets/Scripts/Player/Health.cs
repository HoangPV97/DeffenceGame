using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Health
{
    [SerializeField]
    private float mHealth;
    [SerializeField]
    private float currentHealth;
    [SerializeField]
    private float recoverHealthValue;
    [SerializeField]
    private float recoverHealthTime;
    public Image healthBar;
    public Image effecthealthBar;
    public TextMeshProUGUI healthValueText;
    public float health
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
    public float CurrentHealth
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
    public float RecoverHealthValue
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
    public void Init(float Hp, float HpRegen)
    {
        health = Hp;
        CurrentHealth = health;
        recoverHealthValue = HpRegen;
        UpdateValueText();
        healthBar.fillAmount = 1;
    }
    public void ReduceHealth(float _damage)
    {
        CurrentHealth -= _damage;
        healthBar.fillAmount = CurrentHealth / health * 1.0f;
        UpdateValueText();
    }
    public void EffectHealth()
    {
        effecthealthBar.fillAmount = healthBar.fillAmount;
    }
    public void RecoverHealth()
    {
        CurrentHealth += RecoverHealthValue;
        healthBar.fillAmount += RecoverHealthValue / health;
        if (CurrentHealth > health)
        {
            CurrentHealth = health;
        }
        UpdateValueText();
    }
    public void IncreaseRecoverHealthValue(float _Value)
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
