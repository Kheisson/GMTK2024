using UnityEngine;

namespace _Scripts.Effects
{
    public class ParticleEffectTrigger : MonoBehaviour
    {
        [SerializeField] private GameObject particleEffectPrefab;

        public void TriggerParticleEffect()
        {
            if (particleEffectPrefab == null)
            {
                Debug.LogError("Particle effect prefab is not assigned.");
                return;
            }

            Instantiate(particleEffectPrefab, transform.position, particleEffectPrefab.transform.rotation);
        }
    }
}