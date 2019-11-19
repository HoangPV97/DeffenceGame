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
    public Image maxManaBar;
    // Start is called before the first frame update
    public float maxMana
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
    public void ConsumeMana(float _mana)
    {
        CurrentMana -= _mana;
        float width = maxManaBar.rectTransform.rect.width;
        float height = maxManaBar.rectTransform.rect.height;
        manaBar.rectTransform.sizeDelta = new Vector2(width * (CurrentMana / maxMana * 1.0f), height);
    }
    public void RecoverMana()
    {
        CurrentMana += RecoverManaValue;
        float width = maxManaBar.rectTransform.rect.width;
        float height = maxManaBar.rectTransform.rect.height;
        manaBar.rectTransform.sizeDelta = new Vector2(manaBar.rectTransform.rect.width + (RecoverManaValue / maxMana * width), height);
        if (CurrentMana > maxMana)
        {
            CurrentMana = maxMana;
        }
    }
}

