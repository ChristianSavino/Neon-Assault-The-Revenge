using Keru.Scripts.Engine;
using Keru.Scripts.Engine.Module;
using Keru.Scripts.Game.Effects.Blood;
using Keru.Scripts.Game.Entities.Humanoid;
using UnityEngine;

namespace Keru.Scripts.Game.Entities.Enemy
{
    public class Enemy : Entity
    {
        private ThirdPersonAnimations _animations;

        [SerializeField] private int _maxLife;

        private void Awake()
        {
            _life = _maxLife;
            _animations = GetComponent<ThirdPersonAnimations>();
            _animations.SetConfig();
            _collider = GetComponent<CapsuleCollider>();
            _passiveHandler.SetUp(_animations.GetModelObject());
        }

        public void SetConfig(Difficulty difficulty)
        {

        }

        public override void OnDamagedUnit(int damage, Vector3 hitpoint, GameObject origin, DamageType damageType, float damageForce)
        {
            base.OnDamagedUnit(damage, hitpoint, origin, damageType, damageForce);

            if (_life <= 0)
            {
                if (_alive)
                {
                    Die(hitpoint, damageForce);
                    ApplyDeathEffect(damageType, damageForce, hitpoint);
                }

                ApplyDeathEffect(damageType, damageForce, hitpoint);
            }
            else
            {
                CreateDamageParticle(hitpoint, _alive, damageType);
            }
        }

        private void Die(Vector3 hitpoint, float damageForce)
        {
            _alive = false;
            _collider.enabled = false;
            _animations.Die(hitpoint, damageForce);
        }

        private void ApplyDeathEffect(DamageType damageType, float damageForce, Vector3 hitPoint)
        {
            switch (damageType)
            {
                case DamageType.EXPLOSION:
                    var gibs = Instantiate(CommonItemsManager.ItemsManager.HumanGibs, hitPoint, Quaternion.identity).GetComponent<GibsSpawner>();
                    gibs.SetExplosionGibs(hitPoint, damageForce);
                    Destroy(gameObject);
                    break;
                case DamageType.FIRE:

                    break;
                case DamageType.POISON:

                    break;
                default:
                    _animations.ApplyForceToClosestCollider(hitPoint, damageForce);
                    break;
            }
        }
    }
}
