using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    [System.Serializable]
    public class ObjectPool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }
    public static ObjectPoolManager Instance;
    public List<ObjectPool> Pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        foreach(ObjectPool pool in Pools)
        {
            Queue<GameObject> objectPools = new Queue<GameObject>();
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab) as GameObject;
                obj.SetActive(false);
                objectPools.Enqueue(obj);
            }
            poolDictionary.Add(pool.tag, objectPools);
        }
    }
    public GameObject SpawnObject(string tag, Vector3 _position,Quaternion _quaternion )
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning(" Tag Doesn't Exist");
            return null;
        }
        GameObject objectSpawn= poolDictionary[tag].Dequeue();
        objectSpawn.SetActive(true);
        objectSpawn.transform.position = _position;
        objectSpawn.transform.rotation = _quaternion;
        objectSpawn.transform.parent = this.transform;
        poolDictionary[tag].Enqueue(objectSpawn);
        return objectSpawn;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
