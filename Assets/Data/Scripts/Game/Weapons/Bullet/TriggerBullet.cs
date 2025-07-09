using Keru.Scripts.Game.Entities;
using UnityEngine;

namespace Keru.Scripts.Game.Weapons
{
    [RequireComponent(typeof(Rigidbody))]
    public class TriggerBullet : Bullet
    {
        [SerializeField] private float _lifeTime;

        public override void SetUp(int damage, float force, GameObject owner)
        {
            base.SetUp(damage, force, owner);
            Destroy(gameObject, _lifeTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!_affectsPlayer && other.CompareTag("Player"))
            {
                return;
            }
            var entity = other.GetComponent<Entity>();
            if (entity != null)
            {
                entity.OnDamagedUnit(_damage, transform.position, _owner, _damageType, _force);
            }
            if (_action != null)
            {
                _action.Execute();
            }
        }
    }
}

