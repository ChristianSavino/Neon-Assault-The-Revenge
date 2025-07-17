using Keru.Scripts.Game.ScriptableObjects;
using UnityEngine;

namespace Keru.Scripts.Game.Entities.Passives
{
    public class Passive : MonoBehaviour
    {
        protected PassiveStats _passiveStats;
        protected int _power;
        protected Entity _entity; 
        protected PassiveHandler _passiveHandler;

        public virtual void SetUp(PassiveStats passiveStats, int power, Entity entity)
        {
            _passiveStats = passiveStats;
            _power = power;
            _entity = entity;
            _passiveHandler = gameObject.GetComponentInChildren<PassiveHandler>();
        }

        public PassiveStats GetStats()
        {
            return _passiveStats;
        }

        public virtual void ExecutePassive()
        {

        }

        protected virtual void SetUpEffect()
        {
            if (_passiveStats.Effects != null)
            {
                var particles = Instantiate(_passiveStats.Effects, gameObject.transform);
                var particleSystem = particles.GetComponent<ParticleSystem>();
                var main = particleSystem.main;
                main.duration = _passiveStats.Duration;
            }
        }
    }
}