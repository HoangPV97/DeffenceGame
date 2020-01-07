using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIHeroItem : MonoBehaviour
{
    public Elemental elemental;
    public Image Icon;
    public TextMeshProUGUI txtLevel, txtUnlock;
    public GameObject[] Star;
    public GameObject Lock, Equip;
    public GameObject[] Selected;
    public UIButton btnButton;
    public Animator Animator;
    // Start is called before the first frame update
    public bool IsHero
    {
        get
        {
            return MenuController.Instance.UIPanelHeroAlliance.isHero;
        }
    }
    void Start()
    {
        btnButton.SetUpEvent(OnSelected);
    }

    public void OnUnSelect()
    {
        gameObject.SetActive(false);
        gameObject.SetActive(true);
        Animator.Play("HeroItemDown");
        Selected[0].SetActive(false);
        Selected[1].SetActive(false);
    }

    public void OnSelected()
    {
        Debug.Log(elemental.ToString());
        gameObject.SetActive(false);
        gameObject.SetActive(true);
        Animator.Play("HeroItemUp");
        MenuController.Instance.UIPanelHeroAlliance.OnSelectHero(this);
        Selected[0].SetActive(true);
        Selected[1].SetActive(true);
    }
    public void SetupData()
    {
        gameObject.SetActive(false);
        gameObject.SetActive(true);

        Animator.Play("HeroItemDefault");
        Equip.gameObject.SetActive(false);
        Selected[0].SetActive(false);
        Selected[1].SetActive(false);
        string unlock = "Clear - {0}\n to \n Unlock";
        int LevelUnlock = 0;
        GameDataWeapon weapon;

        if (IsHero)
        {
            weapon = DataController.Instance.GetGameDataWeapon(elemental);
            Equip.gameObject.SetActive(elemental == DataController.Instance.CurrentSelectedWeapon);
            if (elemental == Elemental.Earth)
            {
                LevelUnlock = 10;
            }
            if (elemental == Elemental.Ice)
            {
                LevelUnlock = 20;
            }
            if (elemental == Elemental.Fire)
            {
                LevelUnlock = 30;
            }
        }
        else
        {
            weapon = DataController.Instance.GetGameAlliance(elemental);
            Equip.gameObject.SetActive(elemental == DataController.Instance.ElementalSlot1 || elemental == DataController.Instance.ElementalSlot2);
            if (elemental == Elemental.Wind)
            {
                LevelUnlock = 5;
            }
            if (elemental == Elemental.Earth)
            {
                LevelUnlock = 15;
            }
            if (elemental == Elemental.Ice)
            {
                LevelUnlock = 25;
            }
            if (elemental == Elemental.Fire)
            {
                LevelUnlock = 35;
            }
        }
        txtUnlock.text = string.Format(unlock, LevelUnlock);
        Lock.SetActive(weapon.WeaponTierLevel.Tier == 1 && weapon.WeaponTierLevel.Level == 0);
        txtLevel.gameObject.SetActive(weapon.WeaponTierLevel.Level != 0);
        txtLevel.text = "Lv." + weapon.WeaponTierLevel.Level;
        for (int i = 0; i < Star.Length; i++)
        {
            Star[i].SetActive(i < weapon.WeaponTierLevel.Tier);
        }

    }
}
