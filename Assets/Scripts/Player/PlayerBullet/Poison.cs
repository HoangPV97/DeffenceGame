using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#region poisonData
public class PoisonData
{
    public float totalTime;
    public float inflictedTime;
    public float DamagePerSecond;
    public float Range;


    public PoisonData(float _totalTime, float _inflictTime, float _dps, float _range)
    {
        this.totalTime = _totalTime;
        this.inflictedTime = _inflictTime;
        this.DamagePerSecond = _dps;
        this.Range = _range;
    }
}
#endregion
public class Poison : MonoBehaviour
{
    public PoisonData poison;
    public CircleCollider2D circleCollider2D;
    bool Onstay;
    EnemyController enemy;
    float tempTime;
    // Start is called before the first frame update
    void Start()
    {
        if (poison == null)
        {
            poison = new PoisonData(10, 1.2f, 1, 3.2f);
        }            
        circleCollider2D.radius = poison.Range;
    }

    // Update is called once per frame
    void Update()
    {
        if (Onstay && tempTime < 0 && enemy != null)
        {
            enemy.DealDamge(poison.DamagePerSecond);
            tempTime = poison.inflictedTime;
        }
        tempTime -= Time.deltaTime;
    }

    IEnumerator IEPoison(EnemyController _enemy)
    {
        float _countdown = poison.totalTime;
        while (_countdown > 0)
        {
            yield return new WaitForSeconds(poison.inflictedTime);
            _enemy.DealDamge(poison.DamagePerSecond);
            _countdown -= Time.deltaTime;
        }      
        yield return new WaitForSeconds(poison.totalTime);
    }
    private void OnTriggerStay2D(Collider2D _target)
    {
        if (_target.gameObject.tag.Equals("Enemy"))
        {
            Onstay = true;
            enemy = _target.gameObject.GetComponent<EnemyController>();
        }
    }

    private void OnTriggerEnter2D(Collider2D _target)
    {
        if (_target.gameObject.tag.Equals("Enemy"))
        {
            EnemyController enemy = _target.gameObject.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.DealEffect(Effect.Poiton, enemy.transform.position, poison.totalTime);
                StartCoroutine(IEPoison(enemy));
            }
        }
    }
    private void OnTriggerExit2D(Collider2D _target)
    {
        Onstay = false;
    }
}
