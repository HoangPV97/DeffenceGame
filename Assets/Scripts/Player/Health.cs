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
    float Width,Height;
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
    public void Init()
    {
        Width = healthBar.rectTransform.rect.width;
        Height = healthBar.rectTransform.rect.height;
        CurrentHealth = health;
        UpdateValueText();
    }
    public void ReduceHealth(float _damage)
    {
        CurrentHealth -= _damage;
        healthBar.rectTransform.sizeDelta = new Vector2(Width * (CurrentHealth / health * 1.0f), Height);
        UpdateValueText();
    }
    public void EffectHealth()
    {
        effecthealthBar.rectTransform.sizeDelta = new Vector2(healthBar.rectTransform.rect.width, Height);
    }
    public void RecoverHealth()
    {
        CurrentHealth += RecoverHealthValue;
        healthBar.rectTransform.sizeDelta = new Vector2(healthBar.rectTransform.rect.width + (RecoverHealthValue / health * Width), Height);
        if (CurrentHealth > health)
        {
            CurrentHealth = health;
        }
        UpdateValueText();
    }
    public void UpdateValueText()
    {
        if (healthValueText != null)
        {
            healthValueText.text = currentHealth.ToString() + "/" + health;
        }       
    }
}
