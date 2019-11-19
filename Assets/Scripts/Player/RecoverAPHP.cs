using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecoverAPHP : MonoBehaviour
{
    [SerializeField]
    float recoverIndex, recoverTime;
    [SerializeField]
    Image barImage;
    [SerializeField]
    PlayerController player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //public void RecoverMana()
    //{
    //    if (player.CurrentMana < playerMana)
    //    {
    //        CurrentMana += recoverMana;
    //        manaBar.fillAmount = CurrentMana / Mana * 1.0f;
    //        if (CurrentMana > Mana)
    //            CurrentMana = Mana;
    //    }
    //}
}
