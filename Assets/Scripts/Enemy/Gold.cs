using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{
    RectTransform Rec;
    public float Price { get; set; }
    float Speed;
    // Start is called before the first frame update
    void Start()
    {
        Speed = 100;
        GameObject CanvasGoldPnl = GameObject.FindGameObjectWithTag("GoldPanel");
        Rec = CanvasGoldPnl.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = Rec.position - transform.position;
        transform.Translate(dir.normalized * Speed * Time.deltaTime, Space.World);

    }
    private void OnTriggerEnter2D(Collider2D Target)
    {
        if (Target.gameObject.tag.Equals("GoldPanel"))
        {
            GameController.Instance.GoldUp(Price);
            DestroyImmediate(gameObject);
        }
    }
}
