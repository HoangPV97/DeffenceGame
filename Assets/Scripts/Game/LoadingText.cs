using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingText : MonoBehaviour
{
    public Text Damage;
    public Animation Animation;
    // Start is called before the first frame update

    public void SetTextDamage(string _Damage)
    {
        Damage.text = _Damage;
        Animation.Play();
        StartCoroutine(WaitingDisableObject(0.5f));
    }

    IEnumerator WaitingDisableObject(float _time)
    {
        yield return new WaitForSeconds(_time);
        ObjectPoolManager.Instance.DespawnObJect(gameObject);
    }
}
