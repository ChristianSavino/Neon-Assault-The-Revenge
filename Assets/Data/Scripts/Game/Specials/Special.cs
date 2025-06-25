using Keru.Scripts.Game.Entities.Player;
using Keru.Scripts.Game.Entities.Player.UI;
using Keru.Scripts.Game.ScriptableObjects;
using Keru.Scripts.Game.ScriptableObjects.Models;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace Keru.Scripts.Game.Specials
{
    public class Special : MonoBehaviour
    {
        protected SpecialStats _stats;
        protected SpecialLevel _currentLevel;

        protected int _level;
        protected bool _canCast = true;
        protected Coroutine _coroutine;
        protected Volume _volume;

        protected SpecialDataUIHandler _uiHandler;

        public virtual void SetConfig(SpecialStats stats, int level)
        {
            _stats = stats;
            _level = level;
            _currentLevel = _stats.SpecialLevels[_level - 1];
            if (_stats.Volume != null)
            {
                _volume = new Volume();
                _volume.profile = _stats.Volume;
                _volume.weight = 0f;
            }
        }

        public virtual void SetUIHandler(SpecialDataUIHandler uiHandler, KeyCode keyCode)
        {
            _uiHandler = uiHandler;
            _uiHandler.SetConfig(_stats, _currentLevel, keyCode);
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
            _uiHandler.SetAbilityState(AbilityAction.COOLDOWN);
            yield return new WaitForSeconds(time);
            _canCast = true;
            _uiHandler.SetAbilityState(AbilityAction.IDLE);
        }

        public SpecialStats GetStats()
        {
            return _stats;
        }

        public virtual void Die()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
        }
    }
}
