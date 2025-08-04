using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Keru.Scripts.Game.Effects.Blood
{
    public class BloodSpawn : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _particleSystem;
        [SerializeField] private GameObject _decalPrefab;
        [SerializeField] private List<Material> _materialsOverride;
        [SerializeField] private int _chance;
        [SerializeField] private Vector3 _scale = Vector3.one;

        private void OnParticleCollision(GameObject other)
        {
            var collisionEvents = new List<ParticleCollisionEvent>();
            _particleSystem.GetCollisionEvents(other, collisionEvents);
            foreach (var collision in collisionEvents)
            {
                var randomNumber = Random.Range(0, 100 + 1);
                if (randomNumber <= _chance)
                {
                    CreateBloodDecal(collision, other);
                }
            }
        }

        private void CreateBloodDecal(ParticleCollisionEvent collision, GameObject gameObject)
        {
            Quaternion rotation;
            if (collision.normal != Vector3.zero)
            {
                rotation = Quaternion.LookRotation(collision.normal);
            }
            else
            {
                rotation = Quaternion.LookRotation(Vector3.up);
            }

            var decal = Instantiate(_decalPrefab, collision.intersection, rotation);

            var decalRenderer = decal.GetComponentInChildren<DecalProjector>();
            decalRenderer.material = _materialsOverride[Random.Range(0,_materialsOverride.Count)];
            decalRenderer.size = _scale;
            Destroy(decal, 60);
        }
    }
}
