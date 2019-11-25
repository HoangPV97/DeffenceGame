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
    public Image maxHealthBar;
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
    public void ReduceHealth(float _damage)
    {
        CurrentHealth -= _damage;
        float width = maxHealthBar.rectTransform.rect.width;
        float height = healthBar.rectTransform.rect.height;
        healthBar.rectTransform.sizeDelta = new Vector2(width * (CurrentHealth / health * 1.0f), height);
        UpdateValueText();
    }
    public void RecoverHealth()
    {
        CurrentHealth += RecoverHealthValue;
        float width = maxHealthBar.rectTransform.rect.width;
        float height = healthBar.rectTransform.rect.height;
        healthBar.rectTransform.sizeDelta = new Vector2(healthBar.rectTransform.rect.width + (RecoverHealthValue / health * width), height);
        if (CurrentHealth > health)
        {
            CurrentHealth = health;
        }
        UpdateValueText();
    }
    public void UpdateValueText()
    {
        healthValueText.text = currentHealth.ToString() + "/" + health;
    }
}
