using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateSlider : MonoBehaviour
{
    SpawnEnemy SpawnEnemy;
    // Start is called before the first frame update
    void Start()
    {
        SpawnEnemy = FindObjectOfType<SpawnEnemy>();
        float m_EnemyQuantum = GameController.Instance.EnemyLive;
        LoadingSlider(m_EnemyQuantum);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void LoadingSlider(float _Quantum)
    {
        while (GameController.Instance.EnemyLive > 0)
        {
            float process = GameController.Instance.EnemyLive / _Quantum;
            this.GetComponent<Slider>().value = process;
        }
    }
}
