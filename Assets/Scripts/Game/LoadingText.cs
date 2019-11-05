using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingText : MonoBehaviour
{
    public Text Damage;
    public Animation Animation;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void SetTextDamage(string _Damage)
    {
        Damage.text = _Damage;
        if (!Animation.isPlaying)
        {
            gameObject.SetActive(false);
        }
    }
}
