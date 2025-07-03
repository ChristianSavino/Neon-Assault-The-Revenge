using Keru.Game.Actions.Effects;
using Keru.Scripts.Game.ScriptableObjects;
using System.Collections;
using UnityEngine;

namespace Keru.Scripts.Game.Specials.Overrides
{
    public class BulletTime : Special
    {
        private SlowTime _slowTimeEffect;
        private Coroutine _bulletTimeEffect;

        public override void SetConfig(SpecialStats stats, int level)
        {
            base.SetConfig(stats, level);
        }

        public override bool Execute()
        {
            if (!_canCast)
            {
                return false;
            }

            _coroutine = StartCoroutine(ExecuteBulletTime());
            return true;
        }

        private IEnumerator ExecuteBulletTime()
        {
            _canCast = false;
            Instantiate(_stats.SpecialEffects, transform.position, Quaternion.identity);
            _uiHandler.SetAbilityState(AbilityAction.CAST);

            yield return new WaitForSecondsRealtime(0.417f);

            _uiHandler.SetVolumeProfile(_stats.Volume);
            _uiHandler.SetAbilityState(AbilityAction.TOGGLE, true);
            SetUpBulletTimeEffect();
            _bulletTimeEffect = _slowTimeEffect.DoSlowTime();

            yield return new WaitForSecondsRealtime(_currentLevel.Duration);

            //specialHandler.TurnOffVolume(0.25f * Time.timeScale);

            StartCoroutine(CoolDown(_currentLevel.Cooldown));
        }

        public override void Die()
        {
            base.Die();
            if (_bulletTimeEffect != null)
            {
                StopCoroutine(_bulletTimeEffect);
            }
            Destroy(_slowTimeEffect);
        }

        private void SetUpBulletTimeEffect()
        {
            _slowTimeEffect = gameObject.AddComponent<SlowTime>();
            _slowTimeEffect.SetUp(_currentLevel.Power, _currentLevel.Duration, 0.5f, _uiHandler.GetVolume());
        }
    }
}
