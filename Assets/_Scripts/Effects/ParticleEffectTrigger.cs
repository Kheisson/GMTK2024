using UnityEngine;

namespace _Scripts.Effects
{
    public class ParticleEffectTrigger : MonoBehaviour
    {
        [SerializeField] private GameObject particleEffectPrefab;
        [SerializeField] private float particleLifetime = 2f;

        public void TriggerParticleEffect()
        {
            if (particleEffectPrefab == null)
            {
                Debug.LogError("Particle effect prefab is not assigned.");
                return;
            }

            var particles = Instantiate(particleEffectPrefab, transform.position, particleEffectPrefab.transform.rotation);
            Destroy(particles, particleLifetime);
        }
    }
}