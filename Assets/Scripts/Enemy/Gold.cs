using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{
    GameObject CanvasGoldPnl;
    public float Price { get; set; }
    float Speed;
    Animation aniamtion;
    // Start is called before the first frame update
    void Start()
    {
        Speed = 10.0f;
        CanvasGoldPnl = GameObject.FindGameObjectWithTag("GoldPanel");
        aniamtion = GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    private void OnTriggerEnter2D(Collider2D Target)
    {
        if (Target.gameObject.tag.Equals("GoldPanel"))
        {
            GameController.Instance.GoldUp(Price);
            gameObject.SetActive(false);
        }
    }
    private void Move()
    {
        if (!aniamtion.isPlaying)
        {
            Vector2 dir = CanvasGoldPnl.transform.position - transform.position;
            transform.Translate(dir.normalized * Speed * Time.deltaTime);
        }
    }
}
