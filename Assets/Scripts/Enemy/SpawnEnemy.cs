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
    float process;
    [SerializeField]
    List<Vector2> spawnPosition;
    ObjectPoolManager poolManager;
    // Start is called before the first frame update
    void Start()
    {
        temp = EnemyQuantum;
        poolManager = ObjectPoolManager.Instance;
      
    }

    // Update is called once per frame
    void Update()
    {
        process = 1 - (EnemyQuantum / temp);
        slider.value = process;
    }

}
