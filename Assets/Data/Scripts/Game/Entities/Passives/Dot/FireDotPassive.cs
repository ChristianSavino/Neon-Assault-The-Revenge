using Keru.Scripts.Game.ScriptableObjects;
using System.Collections;
using UnityEngine;

namespace Keru.Scripts.Game.Entities.Passives
{
    public class FireDotPassive : Passive
    {
        private Color _fireColor;
        
        public override void ExecutePassive()
        {
            StartCoroutine(DealDamage());
        }

        public void SetFireColor(Color fireColor)
        {
            _fireColor = fireColor;

            var main = _particles.main;
            main.startColor = _fireColor;
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
    }
}
