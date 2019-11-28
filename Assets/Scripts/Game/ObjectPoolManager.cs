using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InviGiant.Tools;
public class ObjectPoolManager : MonoBehaviour
{
    [System.Serializable]
    public class ObjectPool
    {
        public string tag;
        public GameObject prefab;
    }
    public static ObjectPoolManager Instance;
    public List<ObjectPool> Pools;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    // Start is called before the first frame update
    public GameObject GetGameObjectByTag(string tag)
    {
        for (int i = 0; i < Pools.Count; i++)
            if (Pools[i].tag == tag)
                return Pools[i].prefab;
        return null;
    }
    public GameObject SpawnObject(string tag, Vector3 _position, Quaternion _quaternion)
    {
        return SmartPool.Instance.Spawn(GetGameObjectByTag(tag), _position, _quaternion);
    }

    public GameObject SpawnObject(GameObject prefab, Vector3 _position, Quaternion _quaternion)
    {
        return SmartPool.Instance.Spawn(prefab, _position, _quaternion);
    }

    public void DespawnObJect(GameObject obj)
    {
        SmartPool.Instance.Despawn(obj);
    }

}
