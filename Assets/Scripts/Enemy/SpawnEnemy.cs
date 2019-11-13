using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnEnemy : MonoBehaviour
{
    public Vector3 SpawnPosition;
    public float SpawnTime;
    public Image slider;
    public float EnemyQuantum;
    float temp;
    float process;
    ObjectPoolManager poolManager;
    // Start is called before the first frame update
    void Start()
    {
        Enemy.EnemyLive = EnemyQuantum;
        temp = EnemyQuantum;
        poolManager = ObjectPoolManager.Instance;
        StartCoroutine(WaitForSpawnEnemy());
    }

    // Update is called once per frame
    void Update()
    {
        process = 1 - (EnemyQuantum / temp);
        slider.fillAmount = process;
    }
    IEnumerator WaitForSpawnEnemy()
    {
        while (EnemyQuantum > 0)
        {
            int randomEnemy = Random.Range(1, 4);
            Vector3 RandomPosition = new Vector3(Random.Range(-SpawnPosition.x, SpawnPosition.x), 5.7f, 0);
            GameObject m_Enemy = poolManager.SpawnObject("enemy" + randomEnemy, RandomPosition + transform.TransformPoint(0, 0, 0),transform.rotation);
            //m_Enemy.GetComponent<Enemy>().UpSpeedEnemy(slider.value);
            
            EnemyQuantum--;
            yield return new WaitForSeconds(SpawnTime - 3 * process);
        }
    }
}
