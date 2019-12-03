using UnityEngine;
namespace InviGiant.Tools
{
    public class AutoDespawnParticle : MonoBehaviour
    {

        private ParticleSystem ps;
        public bool isGotParrent = false;

        public void Start()
        {
            ps = GetComponent<ParticleSystem>();

        }

        public void Update()
        {
            if (ps)
            {
                if (!ps.IsAlive())
                {
                    if (!isGotParrent)
                        SmartPool.Instance.Despawn(gameObject);
                    else
                        SmartPool.Instance.Despawn(gameObject.transform.parent.gameObject);
                }
            }
        }
    }
}