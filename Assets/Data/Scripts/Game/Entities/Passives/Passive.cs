using Keru.Scripts.Game.ScriptableObjects;
using System.Collections;
using UnityEngine;

namespace Keru.Scripts.Game.Entities.Passives
{
    public class Passive : MonoBehaviour
    {
        protected PassiveStats _passiveStats;
        protected int _power;
        protected Entity _entity; 
        protected PassiveHandler _passiveHandler;
        protected ParticleSystem _particles;

        public virtual void SetUp(PassiveStats passiveStats, int power, Entity entity)
        {
            _passiveStats = passiveStats;
            _power = power;
            _entity = entity;
            _passiveHandler = gameObject.GetComponentInChildren<PassiveHandler>();
            SetUpEffect();
        }

        public PassiveStats GetStats()
        {
            return _passiveStats;
        }

        public virtual void ExecutePassive()
        {
            _passiveHandler.UpdatePassives();
            StartCoroutine(Duration());
        }

        private void OnDestroy()
        {
            _passiveHandler.DestroyPassive(this);
        }

        private IEnumerator Duration()
        {           
            if (_passiveStats.UsesRealTime)
            {
                yield return new WaitForSecondsRealtime(_passiveStats.Duration);
            }
            else
            {
                yield return new WaitForSeconds(_passiveStats.Duration);
            }
            Destroy(this);
        }

        public virtual void SetUpEffect()
        {
            if (_passiveStats.Effects != null)
            {
                var particles = Instantiate(_passiveStats.Effects, gameObject.transform);
                particles.name = _passiveStats.Effects.name;
                _particles = particles.GetComponent<ParticleSystem>();
                _particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

                var main = _particles.main;
                main.duration = _passiveStats.Duration;

                if(_passiveStats.IsMeshParticle)
                {
                    var shape = _particles.shape;
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

                _particles.Play(true);
            }
        }

        public virtual void Die()
        {
            _particles.Stop();
        }

        public int Power()
        {
            return _power;
        }
    }
}