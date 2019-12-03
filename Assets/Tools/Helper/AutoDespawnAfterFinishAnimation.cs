using System.Collections;
using UnityEngine;
namespace InviGiant.Tools
{
    public class AutoDespawnAfterFinishAnimation : MonoBehaviour
    {
        public float delay = 0f;
        public string animationName;
        public bool PlayOnAwake = true;
        public Animator Animator;
        void Awake()
        {
            Animator = GetComponentInChildren<Animator>();
        }
        void OnEnable()
        {
            if (PlayOnAwake)
            {
                StartCoroutine(PlayAnimation());
            }
        }
        public IEnumerator PlayAnimation()
        {
            //Animator.
            Animator.Play(animationName);
            if (delay != 0)
            {
                yield return new WaitForSeconds(delay);
                SmartPool.Instance.Despawn(this.gameObject);
            }
            else
            {
                delay = Animator.GetCurrentAnimatorStateInfo(0).length;
                yield return new WaitForSeconds(delay);
                SmartPool.Instance.Despawn(this.gameObject);
            }


        }
    }
}