using UnityEngine;

namespace Keru.Scripts.Game.Effects.Blood
{
    public class GibsSpawner : MonoBehaviour
    {
        [SerializeField] PhysicMaterial _gibsMaterial;

        public void SetExplosionGibs(Vector3 explosionPoint, float explosionForce)
        {
            var rigidbody = GetComponentsInChildren<Rigidbody>();
            foreach (var rb in rigidbody)
            {
                rb.AddExplosionForce(explosionForce, explosionPoint, 10f);
                rb.GetComponent<Collider>().material = _gibsMaterial;
                Destroy(rb.gameObject, 60f);
            }
        }
    }
}