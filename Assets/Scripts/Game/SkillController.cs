using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    ObjectPoolManager poolManager;
    PlayerController playerController;
    public string Bullet_Skill_1;
    // Start is called before the first frame update
    void Start()
    {
        poolManager = ObjectPoolManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Skill1Player(Vector2 _direction, float _rotatioZ)
    {
        GameObject skill_1_player = poolManager.SpawnObject(Bullet_Skill_1, gameObject.transform.position, Quaternion.identity);
        skill_1_player.transform.rotation = Quaternion.Euler(0, 0, _rotatioZ);
        Rigidbody2D rigidbody = skill_1_player.GetComponent<Rigidbody2D>();
        float speed = skill_1_player.GetComponent<BulletController>().bullet.Speed;
        rigidbody.velocity = _direction.normalized * 40 * speed * Time.deltaTime;
    }
}
