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
    [SerializeField]
    List<Vector2> spawnPosition;
    ObjectPoolManager poolManager;
    // Start is called before the first frame update
    void Start()
    {
        EnemyController.EnemyLive = EnemyQuantum;
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
            int randomEnemy = Random.Range(1, 6);
            int RandomPosition = Random.Range(0, spawnPosition.Count);
            GameObject m_Enemy = poolManager.SpawnObject("enemy" + randomEnemy, spawnPosition[RandomPosition],transform.rotation);
            //m_Enemy.GetComponent<Enemy>().UpSpeedEnemy(slider.value);
            
            EnemyQuantum--;
            yield return new WaitForSeconds(SpawnTime - 3 * process);
        }
    }
}
