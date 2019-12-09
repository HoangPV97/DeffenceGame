using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Effect { Stun, Freeze, Slow, destroyFreeze, destroyStun };
public class GameEffect : MonoBehaviour
{

    public GameObject GetEffect(Effect _effect, Vector3 _position, float _time)
    {
        GameObject effectObj = null;
        switch (_effect)
        {
            case Effect.Freeze:
                effectObj = SpawnEffect("freezeffect", _position, _time);
                break;
            case Effect.Slow:
                //this.gameObject.GetComponent<Rigidbody2D>().AddForce(_position * 5);
                KnockBack(this.gameObject, _position);
                break;
            case Effect.Stun:
                effectObj = SpawnEffect("stuneffect", _position, _time);
                break;
            case Effect.destroyFreeze:
                effectObj = SpawnEffect("iceeffectend", _position, _time);
                break;
        }
        return effectObj;
    }
    public void KnockBack(GameObject _gameObject, Vector3 _backSpace)
    {
        _gameObject.transform.Translate(_backSpace);
    }
    public GameObject SpawnEffect(string _effectName, Vector3 _position, float _time)
    {
        GameObject effectObj;
        effectObj = ObjectPoolManager.Instance.SpawnObject(_effectName, _position, Quaternion.identity);
        var de = effectObj.GetComponent<DestroyEffect>();
        if (de != null)
            de.StartWaitingDestroyEffect(_time);
        else
            effectObj.AddComponent<DestroyEffect>()._time = _time;
        return effectObj;
    }
    
}
public class DestroyEffect : MonoBehaviour
{
    public float _time;
    public void Start()
    {
        StartCoroutine(WaitingDestroyEffect(_time));
    }
    public void StartWaitingDestroyEffect(float delayTime)
    {
        StartCoroutine(WaitingDestroyEffect(delayTime));
    }
    IEnumerator WaitingDestroyEffect(float time = 0)
    {
        _time = time;
        yield return new WaitForSeconds(_time);
        // this.gameObject.SetActive(false);
        ObjectPoolManager.Instance.DespawnObJect(gameObject);
    }
}
