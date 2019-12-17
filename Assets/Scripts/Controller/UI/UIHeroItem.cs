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
    void Start()
    {
        btnButton.SetUpEvent(OnSelected);
    }
    public void OnSelected()
    {
        Animator.Play("HeroItemUp");
    }
    public void SetupData()
    {

    }
}
