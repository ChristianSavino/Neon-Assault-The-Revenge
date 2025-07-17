using Keru.Scripts.Engine.Module;
using Keru.Scripts.Game.Entities;
using UnityEngine;

namespace Keru.Scripts.Game.Actions.Entities.Effects
{
    public class Explosion : Action
    {
        [SerializeField] float _radius;
        [SerializeField] private ExplosionType _type;
        [SerializeField] private GameObject _overritePrefab;
        private float _force;
        private int _damage;
        private GameObject _explosionObject;
        private GameObject _owner;
        private bool _affectOwner;

        public override void SetUp(int intParamater, float floatParameter, string stringParameter, GameObject gameObjectParamater, bool boolParameter)
        {
            _damage = intParamater;
            _force = floatParameter;
            _owner = gameObjectParamater;
            _affectOwner = boolParameter;
            if(_overritePrefab != null)
            {
                _explosionObject = _overritePrefab;
            }
            else
            {
                _explosionObject = _type == ExplosionType.LARGE ? CommonItemsManager.ItemsManager.BigExplosionEffect : CommonItemsManager.ItemsManager.SmallExplosionEffect;
            }              
        }

        public override void Execute(GameObject target = null)
        {
            var objects = Physics.OverlapSphere(transform.position, _radius);
            var explosion = Instantiate(_explosionObject);
            explosion.transform.position = transform.position;

            foreach (var obj in objects)
            {
                var entity = obj.GetComponent<Entity>();
                if (entity != null)
                {
                    if (_affectOwner || entity.gameObject != _owner)
                    {
                        var distance = Vector3.Distance(transform.position, obj.transform.position);
                        if(distance >= _radius)
                        {
                            continue;
                        }
                        var realDamage = Mathf.RoundToInt((1 - distance / _radius) * _damage);
                        entity.OnDamagedUnit(realDamage, transform.position, _owner, DamageType.EXPLOSION, _force);
                    }
                }
            }
        }

        private enum ExplosionType
        {
            SMALL,
            LARGE
        }
    }
}
