using Keru.Scripts.Engine.Module;
using Keru.Scripts.Game.Effects.Blood;
using Keru.Scripts.Game.Entities.Passives;
using Keru.Scripts.Game.ScriptableObjects;
using Keru.Scripts.Helpers;
using Keru.Scripts.Visuals.Effects.Dissolve;
using System;
using System.Linq;
using UnityEngine;

namespace Keru.Scripts.Game.Entities
{
    public class Entity : MonoBehaviour
    {
        [SerializeField] protected int _life;
        [SerializeField] protected bool _alive = true;
        [SerializeField] protected PassiveHandler _passiveHandler;
        [Header("Gibs")]
        [SerializeField] protected GameObject _impactParticle;
        [SerializeField] protected GameObject _deadParticle;
        [SerializeField] protected GameObject _gibs;

        protected Collider _collider;
        protected bool _appliedDeathEffect;

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
            var normalizedPos = new Vector3(transform.position.x, hitpoint.y, transform.position.z);
            var direction = normalizedPos - hitpoint;
            if(new[] { DamageType.LIGHTNING, DamageType.EXPLOSION, DamageType.TRUE_DAMAGE, DamageType.FIRE, DamageType.POISON }.Contains(damageType))
            {
                return;
            }

            var relativePos = Vector3.Lerp(normalizedPos, hitpoint, 0.5f);

            if (alive)
            {
                
                Instantiate(_impactParticle, relativePos, Quaternion.LookRotation(direction));
            }
            else
            {
                Instantiate(_deadParticle, relativePos, Quaternion.LookRotation(direction));
            }
        }

        protected void ApplyDeathEffect(DamageType damageType, float damageForce, Vector3 hitPoint)
        {
            if (_appliedDeathEffect)
            {
                return;
            }

            switch (damageType)
            {
                case DamageType.EXPLOSION:
                    _appliedDeathEffect = true;
                    var gibs = Instantiate(_gibs, hitPoint, Quaternion.identity).GetComponent<GibsSpawner>();
                    gibs.SetExplosionGibs(hitPoint, damageForce);
                    Destroy(gameObject);
                    break;
                case DamageType.FIRE:
                    _appliedDeathEffect = true;
                    var dissolve = gameObject.AddComponent<DissolveAllObjects>();
                    var passiveFire = CommonFunctions.FindChild(transform, "Passive Fire");
                    dissolve.SetConfig(passiveFire.GetComponent<ParticleSystem>().main.startColor.color);
                    break;
                case DamageType.POISON:

                    break;
                default:
                    CreateDamageParticle(hitPoint, _alive, damageType);
                    break;
            }
        }
    }
}
