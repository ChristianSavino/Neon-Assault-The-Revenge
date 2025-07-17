using Keru.Scripts.Game.ScriptableObjects;
using System.Collections;
using UnityEngine;

namespace Keru.Scripts.Game.Entities.Passives
{
    public class FireDotPassive : Passive
    {
        private Color _fireColor;
        
        public override void SetUp(PassiveStats passiveStats, int power, Entity entity)
        {
            base.SetUp(passiveStats, power, entity);
            SetUpEffect();
        }

        public override void ExecutePassive()
        {
            StartCoroutine(DealDamage());
        }

        public void SetFireColor(Color fireColor)
        {
            _fireColor = fireColor;
        }

        private IEnumerator DealDamage()
        {
            var secondsPassed = 0;
            _entity.OnDamagedUnit(_power, _entity.transform.position, null, DamageType.FIRE, 0f);
            while (secondsPassed < _passiveStats.Duration)
            {
                yield return new WaitForSeconds(1f);
                secondsPassed++;
                if (_entity != null)
                {
                    _entity.OnDamagedUnit(_power, _entity.transform.position, null, DamageType.FIRE, 0f);
                }
            }

            Destroy(this);
        }

        protected override void SetUpEffect()
        {
            if (_passiveStats.Effects != null)
            {
                var particles = Instantiate(_passiveStats.Effects, gameObject.transform);
                var particleSystem = particles.GetComponent<ParticleSystem>();

                var main = particleSystem.main;
                main.startColor = _fireColor;

                var shape = particleSystem.shape;
                var model = _passiveHandler.GetModel();
                var meshRenderer = model.GetComponentInChildren<MeshRenderer>();
                if (meshRenderer != null)
                {
                    shape.shapeType = ParticleSystemShapeType.MeshRenderer;
                    shape.meshRenderer = meshRenderer;
                }

                var skinnedMeshRenderer = model.GetComponentInChildren<SkinnedMeshRenderer>();
                if (skinnedMeshRenderer != null)
                {
                    shape.shapeType = ParticleSystemShapeType.SkinnedMeshRenderer;
                    shape.skinnedMeshRenderer = skinnedMeshRenderer;
                }
            }
        }
    }
}
