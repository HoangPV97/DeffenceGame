using System.Collections;
using UnityEngine;
namespace InviGiant.Tools
{
    public class AutoDeActiveAfterDelay : MonoBehaviour
    {
        public float delay;
        void OnEnable()
        {
            StartCoroutine(AutoDeActive());
        }
        IEnumerator AutoDeActive()
        {
            yield return new WaitForSeconds(delay);
            SmartPool.Instance.Despawn(gameObject);
        }
    }
}
