using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Effect {None, Stun, Freeze, Slow, destroyFreeze, destroyStun ,Poiton,Knockback,StunBullet};
public class GameEffect : MonoBehaviour
{
    public Effect CurrentEffect;
    private void Start()
    {
        CurrentEffect = Effect.None;
    }
    public void SetEffect(Effect _effect)
    {
        CurrentEffect = _effect;
    }
    public GameObject GetEffect(Effect _effect, Vector3 _position, float _time,GameObject parent=null)
    {
        CurrentEffect = _effect;
        GameObject effectObj = null;
        switch (CurrentEffect)
        {
            case Effect.Freeze:
                effectObj = SpawnEffect("ALLIANCE_ICE_SKILL_IMPACT", _position, _time);
                break;
            case Effect.Slow:
                Debug.Log("SLOW_EFFECT");
                break;
            case Effect.Knockback:
                //this.gameObject.GetComponent<Rigidbody2D>().AddForce(_position * 5);
                KnockBack(this.gameObject, _position);
                break;
            case Effect.Stun:
                effectObj = SpawnEffect("STUN_EFFECT", _position+new Vector3(0,1,0), _time);
                break;
            case Effect.destroyFreeze:
                effectObj = SpawnEffect("ALLIANCE_ICE_SKILL_END", _position, _time);
                break;
            case Effect.Poiton:
                //spawn Effect
                Debug.Log("EFFECT POITON");
                break;
            case Effect.StunBullet:
                //spawn Effect
                effectObj = SpawnEffect("STUN_EFFECT", _position + new Vector3(0, 1, 0), _time);
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
    public GameObject SpawnEffect(GameObject _effectObj, Vector3 _position, float _time)
    {
        GameObject effectObj;
        effectObj = ObjectPoolManager.Instance.SpawnObject(_effectObj, _position, Quaternion.identity);
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
