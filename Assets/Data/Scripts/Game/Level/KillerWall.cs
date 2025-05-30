using Keru.Scripts.Game.Entities;
using UnityEngine;

namespace Keru.Scripts.Game.Level
{
    public class KillerWall : MonoBehaviour
    {
        [SerializeField] private int _damage;
        [SerializeField] private float _force;
        [SerializeField] private DamageType _damageType = DamageType.TRUE_DAMAGE;

        private void OnTriggerEnter(Collider other)
        {
            var entity = other.GetComponent<Entity>();
            if (entity != null)
            {
                entity.OnDamagedUnit(_damage, other.ClosestPointOnBounds(transform.position), gameObject, _damageType, _force);
            }
        }
    }
}
