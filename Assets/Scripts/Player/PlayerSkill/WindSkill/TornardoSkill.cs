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
    public CircleCollider2D circleCollider2D;
    bool Onstay;
    List<EnemyController> enemies;
    float tempTime;
    // Start is called before the first frame update
    void Start()
    {
        enemies = new List<EnemyController>();
        Tornardo = new TornardoData(Tornardo.totalTime, Tornardo.inflictedTime, Tornardo.DamagePerSecond, Tornardo.Range);
        circleCollider2D.radius = Tornardo.Range;
    }
    public void SetTornardoData(float _totalTime, float _inflictTime, float _dps, float _range)
    {
        Tornardo.totalTime = _totalTime;
        Tornardo.inflictedTime = _inflictTime;
        Tornardo.DamagePerSecond = _dps;
        Tornardo.Range = _range;
    }
    // Update is called once per frame
    void Update()
    {
        if (Onstay && tempTime < 0 && enemies.Count > 0)
        {
            for(int i=0;i<enemies.Count;i++)
            {
                if (enemies[i].isLive)
                {
                    enemies[i].DealDamge(Tornardo.DamagePerSecond);
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
            yield return new WaitForSeconds(Tornardo.inflictedTime);
            _enemy.DealDamge(Tornardo.DamagePerSecond);
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
}
