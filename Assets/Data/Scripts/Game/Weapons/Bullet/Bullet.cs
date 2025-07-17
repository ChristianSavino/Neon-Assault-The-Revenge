using Keru.Scripts.Game.Actions;
using Keru.Scripts.Game.Entities;
using Keru.Scripts.Helpers;
using System.Linq;
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
            if (!_affectsPlayer && collision.gameObject.CompareTag("Player"))
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

            BeforeDestroy();
            Destroy(gameObject);
        }

        protected virtual void BeforeDestroy()
        {
            var particleEffects = GetComponentsInChildren<ParticleSystem>();
            var trailEffects = GetComponentsInChildren<TrailRenderer>();
            var gameObjects = particleEffects.Select(x => x.gameObject)
                .Concat(trailEffects.Select(x => x.gameObject));

            if(gameObjects.Any())
            {
                var leftEffects = Instantiate(new GameObject("Bullet Left Effects"));
                leftEffects.transform.position = transform.position;
                leftEffects.transform.forward = transform.forward;
                leftEffects.transform.localScale = transform.localScale;

                CommonFunctions.SetGameObjectParent(leftEffects.transform, gameObjects);
                foreach (var effect in particleEffects)
                {
                    var main = effect.main;
                    main.stopAction = ParticleSystemStopAction.Destroy;
                    main.loop = false;
                }
                foreach (var trail in trailEffects)
                {
                    trail.autodestruct = true;
                    trail.emitting = false;
                }

                Destroy(leftEffects, 10f);
            }
        }
    }
}