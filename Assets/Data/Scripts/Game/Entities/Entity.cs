using Keru.Scripts.Game.Entities.Passives;
using Keru.Scripts.Game.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Keru.Scripts.Game.Entities
{
    public class Entity : MonoBehaviour
    {
        [SerializeField] protected int _life;
        [SerializeField] protected bool _alive = true;
        [SerializeField] protected GameObject _impactParticle;
        [SerializeField] protected GameObject _deadParticle;
        [SerializeField] protected PassiveHandler _passiveHandler;

        protected Collider _collider;

        protected void Start()
        {

        }

        public virtual void OnDamagedUnit(int damage, Vector3 hitpoint, GameObject origin, DamageType damageType, float damageForce)
        {
            _life -= damage;
        }

        public virtual void ForceDeployWeapon()
        {

        }

        public virtual Passive UpdatePassiveValues(PassiveStats stats, int power, Entity owner, Type type = null, PassiveCode? passiveCode = null)
        {
            if(type != null || passiveCode != null)
            {
                return _passiveHandler.AddPassive(stats, power, owner, type, passiveCode);
            }
            else
            {
                _passiveHandler.UpdatePassives();
                return null;
            }
        }

        protected void CreateDamageParticle(Vector3 hitpoint, bool alive, DamageType damageType)
        {
            var direction = hitpoint - transform.position;
            if(new[] { DamageType.LIGHTNING, DamageType.EXPLOSION, DamageType.TRUE_DAMAGE, DamageType.FIRE, DamageType.POISON }.Contains(damageType))
            {
                return;
            }

            if (alive)
            {
                Instantiate(_impactParticle, hitpoint, Quaternion.LookRotation(direction));
            }
            else
            {
                Instantiate(_deadParticle, hitpoint, Quaternion.LookRotation(direction));
            }
        }
    }
}
