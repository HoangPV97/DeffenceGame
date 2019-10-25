using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject[] Enemies;
    public Vector3 SpawnPosition;
    public float SpawnTime;
    bool StopSpawn;
    public Slider slider;
    public float EnemyQuantum;
    float temp;
    // Start is called before the first frame update
    void Start()
    {
        Enemy.EnemyLive = EnemyQuantum;
        temp = EnemyQuantum;
        StopSpawn = false;
        StartCoroutine(WaitForSpawnEnemy());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator WaitForSpawnEnemy()
    {
        while (StopSpawn==false &&EnemyQuantum>0)
        {
            int randomEnemy = Random.Range(0, Enemies.Length);
            Vector3 RandomPosition=new Vector3(Random.Range(-SpawnPosition.x, SpawnPosition.x), 6, 0);
            GameObject m_Enemy= Instantiate(Enemies[randomEnemy], RandomPosition+transform.TransformPoint(0,0,0), Enemies[randomEnemy].transform.rotation,this.gameObject.transform);
            m_Enemy.GetComponent<Enemy>().UpSpeedEnemy(slider.value);
            float process =1-(EnemyQuantum / temp);
            slider.value = process;
            EnemyQuantum--;
            yield return new WaitForSeconds(SpawnTime);
        }
        
    }
}
