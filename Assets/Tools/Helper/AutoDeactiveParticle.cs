using UnityEngine;
namespace InviGiant.Tools
{
    public class AutoDeactiveParticle : MonoBehaviour
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
                        gameObject.SetActive(false);
                    else
                        gameObject.transform.parent.gameObject.SetActive(false);
                }
            }
        }
    }
}