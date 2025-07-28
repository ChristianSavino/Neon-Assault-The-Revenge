using Keru.Scripts.Game.Entities;
using Keru.Scripts.Game.Entities.Humanoid;
using Keru.Scripts.Game.Entities.Passives;
using Keru.Scripts.Game.Entities.Player.UI;
using Keru.Scripts.Game.ScriptableObjects;
using Keru.Scripts.Game.ScriptableObjects.Models;
using System.Collections;
using UnityEngine;

namespace Keru.Scripts.Game.Specials
{
    public class Special : MonoBehaviour
    {
        protected SpecialStats _stats;
        protected SpecialLevel _currentLevel;

        protected int _level;
        protected bool _canCast = true;
        protected Coroutine _coroutine;

        protected SpecialDataUIHandler _uiHandler;
        protected ThirdPersonAnimations _animations;

        protected GameObject _owner;

        public virtual void SetConfig(ThirdPersonAnimations animations, SpecialStats stats, int level, GameObject owner)
        {
            _animations = animations;
            _stats = stats;
            _level = Mathf.Clamp(level, 1, _stats.SpecialLevels.Count);
            _currentLevel = _stats.SpecialLevels[_level - 1];
            _owner = owner;
        }

        public virtual void SetUIHandler(SpecialUIHandler specialUIHandler, SpecialDataUIHandler uiHandler, KeyCode keyCode)
        {
            _uiHandler = uiHandler;
            _uiHandler?.SetConfig(specialUIHandler, _stats, _currentLevel, keyCode);
        }

        public virtual bool Execute()
        {
            if (!_canCast)
            {
                return false;
            }
            return true;
        }

        protected virtual IEnumerator CoolDown(float time)
        {
            if (_uiHandler != null)
            {
                _uiHandler.SetAbilityState(AbilityAction.COOLDOWN);
            }
                
            yield return new WaitForSeconds(time);
            _canCast = true;

            if (_uiHandler != null)
            {
                _uiHandler.SetAbilityState(AbilityAction.IDLE);
            }  
        }

        public SpecialStats GetStats() => _stats;

        public virtual void Die()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
                _coroutine = null;
            }
        }

        public Passive ApplyPassive(PassiveStats stats, int power, Entity owner)
        {
            if (_stats?.Passive == null)
            {
                return null;
            }
                
            var entity = _owner.GetComponentInParent<Entity>();
            var passive = entity.UpdatePassiveValues(stats, power, owner, null, _stats.Passive.Code);
            passive?.SetUp(stats, power, owner);
            return passive;
        }
    }
}
