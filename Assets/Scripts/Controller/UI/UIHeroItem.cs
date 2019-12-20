using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIHeroItem : MonoBehaviour
{
    public Elemental elemental;
    public Image Icon;
    public TextMeshProUGUI txtLevel;
    public GameObject[] Star;
    public GameObject Selected, Lock;
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
        Animator.Play("HeroItemDown");
        Selected.SetActive(false);
    }

    public void OnSelected()
    {
        Animator.Play("HeroItemUp");
        MenuController.Instance.UIPanelHeroAlliance.OnSelectHero(this);
        Selected.SetActive(true);
    }
    public void SetupData()
    {
        gameObject.SetActive(false);
        gameObject.SetActive(true);
        Selected.SetActive(false);
        Animator.Play("HeroItemDefault");
        GameDataWeapon weapon;
        if (IsHero)
            weapon = DataController.Instance.GetGameDataWeapon(elemental);
        else
            weapon = DataController.Instance.GetGameAlliance(elemental);

        Lock.SetActive(weapon.WeaponTierLevel.Tier == 1 && weapon.WeaponTierLevel.Level == 0);
        txtLevel.gameObject.SetActive(weapon.WeaponTierLevel.Level != 0);
        txtLevel.text = "Lv." + weapon.WeaponTierLevel.Level;
        for (int i = 0; i < Star.Length; i++)
        {
            Star[i].SetActive(i < weapon.WeaponTierLevel.Tier);
        }

    }
}
