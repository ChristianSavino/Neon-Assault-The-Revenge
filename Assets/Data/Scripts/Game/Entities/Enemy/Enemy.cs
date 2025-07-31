using Keru.Scripts.Engine;
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
                if(_alive)
                {
                    _collider.enabled = false;
                    Die(hitpoint, damageForce);
                    _alive = false;
                }
                else
                {
                    _animations.ApplyForceToClosestCollider(hitpoint, damageForce);
                }                 
            }

            CreateDamageParticle(hitpoint, _alive, damageType);
        }

        private void Die(Vector3 hitpoint, float damageForce)
        {
            _alive = false;
            _animations.Die(hitpoint, damageForce);
        }
    }
}
