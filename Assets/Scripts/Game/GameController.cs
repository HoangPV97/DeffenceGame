using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public GameObject GameoverPanel,WingamePanel;
    public Text GoldText,WinGold;
    float Gold = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        Time.timeScale = 1;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Player.Live == false)
        {
            EndGame();
        }
        if (Enemy.EnemyLive == 0)
        {
            WinGame();
        }
        
    }
    public void EndGame()
    {
        
        GameoverPanel.SetActive(true);
        Time.timeScale = 0;
    }
    public void Restart()
    {
        
        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
    public void SetGoldText(float _gold)
    {
        GoldText.text = "" + _gold;
        WinGold.text = "Gold : " + _gold;
    }
    public void GoldUp( float _bonusgold)
    {
        Gold += _bonusgold;
        SetGoldText(Gold);
    }
    public void WinGame()
    {
        WingamePanel.SetActive(true);
    }
}
