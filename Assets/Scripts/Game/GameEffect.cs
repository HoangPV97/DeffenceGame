using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Effect { Stun, freeze, Slow };
public class GameEffect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public GameObject GetEffect(Effect _effect, Vector3 _position, float _time)
    {
        GameObject effectObj = null;
        
        switch (_effect)
        {
            case Effect.freeze:
                effectObj = ObjectPoolManager.Instance.SpawnObject("freezeffect", _position, Quaternion.identity);
                effectObj.AddComponent<DestroyFreezeEffect>()._time = _time;
                break;
            case Effect.Slow:
                gameObject.GetComponent<Rigidbody2D>().AddForce(_position*5);
                break;
            case Effect.Stun:
                effectObj = ObjectPoolManager.Instance.SpawnObject("stuneffect", _position, Quaternion.identity);
                effectObj.AddComponent<DestroyFreezeEffect>()._time = _time;
                break;
        }
        return effectObj;
    }
    public void KnockBack( GameObject _gameObject,Vector3 _backSpace)
    {
        _gameObject.transform.position += _backSpace;
    }
}
public class DestroyFreezeEffect:MonoBehaviour
{

    public float _time;
    private void Start()
    {
        StartCoroutine(WaitingDestroyEffect());
    }
    IEnumerator WaitingDestroyEffect()
    {
        
        yield return new WaitForSeconds(_time);
        gameObject.SetActive(false);
    }
}
