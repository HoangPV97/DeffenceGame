using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Effect { Stun, freeze, Slow };
public class GameEffect : MonoBehaviour
{

    public Effect currentEffect;
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
                break;
            case Effect.Slow:
                KnockBack(_position);
                break;
            case Effect.Stun:
                effectObj = ObjectPoolManager.Instance.SpawnObject("stuneffect", _position, Quaternion.identity);
                break;
        }
        return effectObj;
    }
    IEnumerator WaitingDestroyEffect(GameObject _gameObject, float _time)
    {
        yield return new WaitForSeconds(_time);
        _gameObject.SetActive(false);
    }
    public void KnockBack(Vector3 _backSpace)
    {
        transform.position += _backSpace;
    }
}
