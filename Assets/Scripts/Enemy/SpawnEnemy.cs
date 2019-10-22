using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject[] Enemy;
    public Vector3 SpawnPosition;
    public float SpawnTime;
    bool StopSpawn;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitForSpawnEnemy());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator WaitForSpawnEnemy()
    {
        while (!StopSpawn)
        {
            int randomEnemy = Random.Range(0,Enemy.Length);
            Vector3 RandomPosition=new Vector3(Random.Range(-SpawnPosition.x, SpawnPosition.x), 6, 0);
            Instantiate(Enemy[randomEnemy], RandomPosition+transform.TransformPoint(0,0,0), Enemy[randomEnemy].transform.rotation,this.gameObject.transform);
            yield return new WaitForSeconds(SpawnTime);
        }
        
    }
}
