using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnEnemy : MonoBehaviour
{
    public Vector3 SpawnPosition;
    public float SpawnTime;
    public Slider slider;
    public float EnemyQuantum;
    float temp;
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

    }
    IEnumerator WaitForSpawnEnemy()
    {
        while (EnemyQuantum > 0)
        {
            int randomEnemy = Random.Range(1, 4);
            Vector3 RandomPosition = new Vector3(Random.Range(-SpawnPosition.x, SpawnPosition.x), 6, 0);
            GameObject m_Enemy = poolManager.SpawnObject("enemy" + randomEnemy, RandomPosition + transform.TransformPoint(0, 0, 0),transform.rotation);
            //m_Enemy.GetComponent<Enemy>().UpSpeedEnemy(slider.value);
            float process = 1 - (EnemyQuantum / temp);
            slider.value = process;
            EnemyQuantum--;
            yield return new WaitForSeconds(SpawnTime - 3 * process);
        }
    }
}
