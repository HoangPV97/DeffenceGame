using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public TextMeshProUGUI manaValueText;
    float Width, Height;
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
    public void Init(float mn, float ManaRegen)
    {
        Width = manaBar.rectTransform.rect.width;
        Height = manaBar.rectTransform.rect.height;
        maxMana = mn;
        recoverManaValue = ManaRegen;
        CurrentMana = maxMana;
        UpdateValueText();
    }
    public void ConsumeMana(float _mana)
    {
        CurrentMana -= _mana;
        manaBar.rectTransform.sizeDelta = new Vector2(Width * (CurrentMana / maxMana * 1.0f), Height);
        UpdateValueText();
    }
    public void RecoverMana()
    {
        CurrentMana += RecoverManaValue;
        manaBar.rectTransform.sizeDelta = new Vector2(manaBar.rectTransform.rect.width + (RecoverManaValue / maxMana * Width), Height);
        if (CurrentMana > maxMana)
        {
            CurrentMana = maxMana;
        }
        UpdateValueText();
    }
    public void UpdateValueText()
    {
        manaValueText.text = CurrentMana.ToString() + "/" + maxMana.ToString();
    }
}

