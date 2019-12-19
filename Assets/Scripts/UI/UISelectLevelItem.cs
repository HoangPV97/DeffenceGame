using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UISelectLevelItem : MonoBehaviour
{
    public UIButton UIButton;
    public GameObject Lock, Unlock;
    public TextMeshProUGUI txtLevel;
    public GameObject[] Star;
    public int Level;
    [SerializeField]
    GameStage gameStage;
    // Start is called before the first frame update
    void Awake()
    {
        Level = int.Parse(gameObject.name);
        UIButton = GetComponent<UIButton>();
    }
    private void Start()
    {
        UIButton.SetUpEvent(OnBtnClick);
        // SetUpData();
    }
    public void SetUpData()
    {
        gameStage = DataController.Instance.GetGameStage(Level);
        Lock.SetActive(gameStage.HardMode == 0);
        Unlock.SetActive(gameStage.HardMode != 0);
        txtLevel.text = Level.ToString();
        for (int i = 0; i < Star.Length; i++)
        {
            Star[i].SetActive(gameStage.HardMode >= i + 2);
        }
    }
    void OnBtnClick()
    {
        if (gameStage.HardMode != 0 && gameStage.HardMode < 4)
        {
            MenuController.Instance.UiSelectLevel.SetUpDataUILevelDetail(Level);
        }
    }
}
