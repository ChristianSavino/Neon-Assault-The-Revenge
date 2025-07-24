using Keru.Scripts.Game.Actions.Effects;
using Keru.Scripts.Game.Entities;
using Keru.Scripts.Game.Entities.Humanoid;
using Keru.Scripts.Game.ScriptableObjects;
using System.Collections;
using UnityEngine;

namespace Keru.Scripts.Game.Specials.Overrides
{
    public class BulletTime : Special
    {
        private SlowTime _slowTimeEffect;
        private Coroutine _bulletTimeEffect;

        public override void SetConfig(ThirdPersonAnimations animations, SpecialStats stats, int level, GameObject owner)
        {
            base.SetConfig(animations, stats, level, owner);
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
            SetFireRatePassive();

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

        private void SetFireRatePassive()
        {
            var passive = ApplyPassive(_stats.Passive, Mathf.RoundToInt(_currentLevel.Power * 100), _owner.GetComponentInParent<Entity>());
            passive.ExecutePassive();
        }
    }
}
