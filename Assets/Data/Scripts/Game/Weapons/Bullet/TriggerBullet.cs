using Keru.Scripts.Game.Actions;
using Keru.Scripts.Game.Entities;
using System.Collections;
using UnityEngine;

namespace Keru.Scripts.Game.Weapons
{
    [RequireComponent(typeof(Rigidbody))]
    public class TriggerBullet : Bullet
    {
        [SerializeField] private float _lifeTime;
        [SerializeField] private Action _hitAction;

        public override void SetUp(int damage, float force, GameObject owner)
        {
            base.SetUp(damage, force, owner);
            StartCoroutine(DestroyBullet());
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
            if (_hitAction != null)
            {
                _hitAction.Execute(other.gameObject);
            }
        }

        private IEnumerator DestroyBullet()
        {
            yield return new WaitForSeconds(_lifeTime);
            BeforeDestroy();

            if (_action != null)
            {
                _action.Execute();
            }

            Destroy(gameObject);
        }
    }
}

