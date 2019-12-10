using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingText : MonoBehaviour
{
    public TextMeshProUGUI Damage;
    public Animation Animation;
    // Start is called before the first frame update
    void Start()
    {
    }
    private void Update()
    {
        gameObject.transform.position = Vector3.MoveTowards(transform.position, transform.position + new Vector3(0, 0.05f, 0), 2f * Time.deltaTime);
    }

    public void SetTextDamage(string _Damage)
    {
        Damage.text = "-"+_Damage;
        if (!Animation.isPlaying)
        {
            gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// Animation event
    /// </summary>
    public void DespawnObject()
    {
        ObjectPoolManager.Instance.DespawnObJect(gameObject);
    }
}
