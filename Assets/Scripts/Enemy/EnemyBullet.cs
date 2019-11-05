using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : Bullet
{
    public void SetDamage(float _damage)
    {
        Damge = _damage;
    }
    public void SetSpeed(float _speed)
    {
        Speed = _speed;
    }
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
    }
    private void OnTriggerEnter2D(Collider2D Player)
    {
        if (Player.gameObject.tag.Equals(TargetTag))
        {
            Player player = Player.GetComponent<Player>();
            player?.TakeDamge(Damge);
            if (SeekTarget)
            {
                gameObject.SetActive(false);
            }
        }
    }
    
}
