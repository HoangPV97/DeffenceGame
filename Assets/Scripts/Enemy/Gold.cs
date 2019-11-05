using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{
    RectTransform Rec;
    public float Price { get; set; }
    float Speed;
    Animation aniamtion;
    // Start is called before the first frame update
    void Start()
    {
        Speed = 200.0f;
        GameObject CanvasGoldPnl = GameObject.FindGameObjectWithTag("GoldPanel");
        Rec = CanvasGoldPnl.GetComponent<RectTransform>();
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
    IEnumerator WaitforMove()
    {
        yield return new WaitForSeconds(2.0f);
    }
    private void Move()
    {
        if (!aniamtion.isPlaying)
        {
            Vector3 dir = Rec.position - transform.position;
            transform.Translate(dir.normalized * Speed * Time.deltaTime, Space.World);
        }

    }
}
