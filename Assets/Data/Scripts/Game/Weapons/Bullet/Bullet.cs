using Keru.Game.Actions;
using Keru.Scripts.Game.Entities;
using UnityEngine;

namespace Keru.Scripts.Game.Weapons
{
    [RequireComponent(typeof(Rigidbody))]
    public class Bullet : MonoBehaviour
    {
        [SerializeField] protected float _speed;
        [SerializeField] protected Action _action;
        [SerializeField] protected DamageType _damageType;
        [SerializeField] protected bool _usesGravity;
        [SerializeField] protected bool _affectsPlayer;

        protected int _damage;
        protected float _force;
        protected GameObject _owner;

        public virtual void SetUp(int damage, float force, GameObject owner)
        {
            _damage = damage;
            _force = force;
            _owner = owner;

            var rb = GetComponent<Rigidbody>();
            rb.velocity = transform.forward * _speed;
            rb.useGravity = _usesGravity;

            if (_action != null)
            {
                _action.SetUp(_damage, _force, string.Empty, _owner, _affectsPlayer);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(!_affectsPlayer && collision.gameObject.CompareTag("Player"))
            {
                return;
            }

            var entity = collision.gameObject.GetComponent<Entity>();

            if (entity != null)
            {
                entity.OnDamagedUnit(_damage, collision.contacts[0].point, _owner, _damageType, _force);
            }

            if (_action != null)
            {
                _action.Execute();
            }

            Destroy(gameObject);
        }
    }
}