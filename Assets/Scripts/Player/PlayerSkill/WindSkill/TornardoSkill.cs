using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TornardoData
{
    public float totalTime;
    public float DamagePerSecond;
    public float inflictedTime;
    public float Range;
    public TornardoData(float _totalTime, float _inflictTime, float _dps, float _range)
    {
        this.totalTime = _totalTime;
        this.inflictedTime = _inflictTime;
        this.DamagePerSecond = _dps;
        this.Range = _range;
    }
}
public class TornardoSkill : MonoBehaviour
{
    public TornardoData Tornardo;
    public ParticleScaler ParticleScaler;
    public ParticleSystem ParticleSystem;
    bool Onstay;
    List<EnemyController> enemies;
    float tempTime;
    // Start is called before the first frame update
    void Start()
    {
        Tornardo = new TornardoData(Tornardo.totalTime, Tornardo.inflictedTime, Tornardo.DamagePerSecond, Tornardo.Range);
        ParticleScaler.ScaleByTransform(ParticleSystem, Tornardo.Range * 0.3f, true);
    }

    public void SetTornardoData(float _totalTime, float _inflictTime, float _dps, float _range)
    {
        enemies = new List<EnemyController>();
        Tornardo.totalTime = _totalTime;
        //Tornardo.inflictedTime = _inflictTime;
        Tornardo.DamagePerSecond = _dps;
        Tornardo.Range = _range;
    }

    // Update is called once per frame
    void Update()
    {
        if (Onstay && tempTime < 0 && enemies.Count > 0)
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].isLive)
                {
                    Hurted(enemies[i]);
                }
                else
                {
                    enemies.RemoveAt(i);
                }
            }
            tempTime = Tornardo.inflictedTime;
        }
        tempTime -= Time.deltaTime;
    }

    IEnumerator IETornardo(EnemyController _enemy)
    {
        float _countdown = Tornardo.totalTime;
        while (_countdown > 0)
        {
            yield return new WaitForSeconds(1f);
            _enemy.gameEffect.SpawnEffect("HERO_WIND_BULLET_IMPACT", _enemy.transform.position, 0.5f);
            _enemy.DealDamge((int)Tornardo.DamagePerSecond);
            _countdown -= Time.deltaTime;
        }
        yield return new WaitForSeconds(Tornardo.totalTime);
    }

    private void OnTriggerStay2D(Collider2D _target)
    {
        if (_target.gameObject.tag.Equals("Enemy"))
        {
            Onstay = true;
            EnemyController enemy = _target.gameObject.GetComponent<EnemyController>();
            if (!enemies.Contains(enemy))
            {
                enemies.Add(enemy);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D _target)
    {
        if (_target.gameObject.tag.Equals("Enemy"))
        {

            Onstay = true;
            EnemyController enemy = _target.gameObject.GetComponent<EnemyController>();
            //enemy.DealDamge((int)Tornardo.DamagePerSecond);
            if (!enemies.Contains(enemy))
            {
                enemies.Add(enemy);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D _target)
    {
        Onstay = false;
        EnemyController enemy = _target.gameObject.GetComponent<EnemyController>();
        if (enemies.Contains(enemy))
        {
            enemies.Remove(enemy);
        }
    }
    public void Hurted(EnemyController _enemyCtr)
    {
        _enemyCtr.gameEffect.SpawnEffect("HERO_WIND_BULLET_IMPACT", _enemyCtr.transform.position, 0.5f);
        if (_enemyCtr.enemy.elemental.Equals(Elemental.Wind) && _enemyCtr.enemy.Resistance)
        {
            _enemyCtr.DealDamge((int)(Tornardo.DamagePerSecond / 2));
        }
        else if (_enemyCtr.enemy.elemental.Equals(Elemental.Earth))
        {
            float damageplus = DataController.Instance.inGameWeapons.ATKplus;
            _enemyCtr.DealDamge((int)(Tornardo.DamagePerSecond),damageplus);
        }
        else
        {
            _enemyCtr.DealDamge((int)(Tornardo.DamagePerSecond));
        }
    }
}
