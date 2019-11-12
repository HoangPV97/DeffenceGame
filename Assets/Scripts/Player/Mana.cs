using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Mana
{
    [SerializeField]
    private float mMana;
    [SerializeField]
    private float currentMana;
    [SerializeField]
    private float recoverManaValue;
    [SerializeField]
    private float recoverManaTime;
    public Image manaBar;
    // Start is called before the first frame update
    public float mana
    {
        get
        {
            return this.mMana;
        }
        set
        {
            this.mMana = value;
        }
    }
    public float CurrentMana
    {
        get
        {
            return this.currentMana;
        }
        set
        {
            this.currentMana = value;
        }
    }
    public float RecoverManaValue
    {
        get
        {
            return this.recoverManaValue;
        }
        set
        {
            this.recoverManaValue = value;
        }
    }
    public float RecoverManaTime
    {
        get
        {
            return this.recoverManaTime;
        }
        set
        {
            this.recoverManaTime = value;
        }
    }
}
