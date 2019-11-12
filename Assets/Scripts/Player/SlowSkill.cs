using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowSkill : Bullet
{
    [SerializeField]
    float backSpace;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D _Target)
    {
        if (_Target.gameObject.tag.Equals(TargetTag))
        {
            Enemy enemy = _Target.GetComponent<Enemy>();
            enemy?.TakeDamage(elementalBullet, Damge, damagePlus);
            enemy?.Back(backSpace); 
            
            if (SeekTarget)
            {

                gameObject.SetActive(false);
            }
        }
    }
}
