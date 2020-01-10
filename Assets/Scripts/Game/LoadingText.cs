using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LoadingText : MonoBehaviour
{
    public TextMeshPro Damage;
    public Animation Animation;
    // Start is called before the first frame update

    public void SetTextDamage(string _Damage)
    {
        gameObject.layer = 1;
        if (Damage == null)
            Damage = GetComponent<TextMeshPro>();
        Damage.text = _Damage;
        Animation.Play();
        if (gameObject.activeInHierarchy)
            StartCoroutine(WaitingDisableObject(0.5f));
    }

    IEnumerator WaitingDisableObject(float _time)
    {
        yield return new WaitForSeconds(_time);
       
            ObjectPoolManager.Instance.DespawnObJect(gameObject);
    }
}
