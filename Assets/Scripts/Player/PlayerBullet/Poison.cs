using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonData
{
    public float totalTime;
    public float inflictedTime;
    public float DamagePerSecond;
    public float Range;
}
public class Poison : MonoBehaviour
{
    public PoisonData poison;
    CircleCollider2D circleCollider2D;
    // Start is called before the first frame update
    void Start()
    {
        circleCollider2D.radius = poison.Range;
        DelayDespawn(poison.totalTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    IEnumerator IEPoison(EnemyController _enemy)
    {
        yield return new WaitForSeconds(poison.inflictedTime/5);
        _enemy.DealDamge(poison.DamagePerSecond);
    }
    private void OnTriggerStay2D(Collider2D _target)
    {
        Poisoned(_target);
    }
    private void OnTriggerExit2D(Collider2D _target)
    {
        Poisoned(_target);
    }
    protected IEnumerator DelayDespawn(float _time)
    {
        yield return new WaitForSeconds(_time);
        ObjectPoolManager.Instance.DespawnObJect(gameObject);
    }
    public void Poisoned( Collider2D _target)
    {
        if (_target.gameObject.tag.Equals("Enemy"))
        {
            Debug.Log("EXPLOSION");
            EnemyController enemy = _target.gameObject.GetComponent<EnemyController>();
            StartCoroutine(IEPoison(enemy));
        }
    }
}
