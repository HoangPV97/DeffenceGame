using System.Collections;
using System.Collections.Generic;
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
}
