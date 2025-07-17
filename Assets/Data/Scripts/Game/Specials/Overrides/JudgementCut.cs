using Keru.Scripts.Game.Entities.Humanoid;
using Keru.Scripts.Game.ScriptableObjects;
using Keru.Scripts.Game.Weapons;
using System.Collections;
using UnityEngine;


namespace Keru.Scripts.Game.Specials.Overrides
{
    public class JudgementCut : Special
    {
        public override void SetConfig(ThirdPersonAnimations animations, SpecialStats stats, int level, GameObject owner)
        {
            base.SetConfig(animations, stats, level,owner);
        }

        public override bool Execute()
        {
            if (!_canCast)
            {
                return false;
            }
            _coroutine = StartCoroutine(ExecuteJudgementCut());
            return true;
        }

        private IEnumerator ExecuteJudgementCut()
        {
            _canCast = false;
            
            //Instantiate(_stats.SpecialEffects, transform.position, Quaternion.identity);
            _uiHandler.SetAbilityState(AbilityAction.CAST);

            yield return new WaitForSeconds(0.6f);

            var cut = Instantiate(_stats.SpecialObject);
            var vector = _animations.GetModelObject().transform.position + _animations.GetModelObject().transform.forward.normalized * 3.9f;
            vector.y += 1f;
            
            cut.transform.position = vector;
            cut.transform.forward = _animations.GetModelObject().transform.forward;
            
            var bullet = cut.GetComponent<Bullet>();
            bullet.SetUp((int)_currentLevel.Power, 20, _owner);

            yield return new WaitForSeconds(0.6f);
            StartCoroutine(CoolDown(_currentLevel.Cooldown));
        }
    }
}