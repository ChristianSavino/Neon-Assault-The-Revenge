using Keru.Scripts.Engine.Helper;
using Keru.Scripts.Game.ScriptableObjects;
using Keru.Scripts.Game.ScriptableObjects.Models;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Keru.Scripts.Game.Entities.Player.UI
{
    public class SpecialDataUIHandler : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Image _bg;
        [SerializeField] private Text _text;
        private SpecialUIHandler _specialUIHandler;

        private SpecialStats _stats;
        private SpecialLevel _specialLevel;

        private KeyCode _keyCode;

        private Coroutine _imageRoutine;
        private Coroutine _timerRoutine;

        public void SetConfig(SpecialUIHandler specialUIHandler, SpecialStats stats, SpecialLevel specialLevel, KeyCode keyCode)
        {
            _specialUIHandler = specialUIHandler;
            _stats = stats;
            _specialLevel = specialLevel;
            _keyCode = keyCode;
            _image.sprite = stats.Icon;
            _bg.sprite = stats.Icon;
            SetAbilityState(AbilityAction.IDLE);
        }

        public void SetAbilityState(AbilityAction action, bool isRealTime = false)
        {
            if (_imageRoutine != null)
            {
                StopCoroutine(_imageRoutine);
            }
            if (_timerRoutine != null)
            {
                StopCoroutine(_timerRoutine);
            }

            switch (action)
            {
                case AbilityAction.IDLE:
                    _image.color = Color.white;
                    _text.color = Color.white;
                    _text.text = StringFormatterHelper.GetFormattedKeyCode(_keyCode);
                    break;
                case AbilityAction.CAST:
                    _image.color = Color.cyan;
                    _text.color = Color.cyan;
                    _text.text = StringFormatterHelper.GetFormattedKeyCode(_keyCode);
                    break;
                case AbilityAction.TOGGLE:
                    _image.color = Color.cyan;
                    _text.color = Color.cyan;
                    StartCountDown(_specialLevel.Duration, false, isRealTime);
                    break;
                case AbilityAction.COOLDOWN:
                    _image.color = Color.red;
                    _text.color = Color.red;
                    StartCountDown(_specialLevel.Cooldown, true, isRealTime);
                    break;
            }
        }

        private void StartCountDown(float duration, bool isIncrease, bool isRealTime = false)
        {
            _imageRoutine = StartCoroutine(CountDownFillerImage(duration, isIncrease, isRealTime));
            _timerRoutine = StartCoroutine(CountDown(duration, isRealTime));
        }

        private IEnumerator CountDownFillerImage(float duration, bool isIncrease, bool isRealTime)
        {
            var elapsed = 0f;
            var start = 0f;
            var end = 1f;
            while (elapsed < duration)
            {
                if (isRealTime)
                {
                    elapsed += Time.unscaledDeltaTime;
                }
                else
                {
                    elapsed += Time.deltaTime;
                }

                if (isIncrease)
                {
                    _image.fillAmount = Mathf.Lerp(start, end, elapsed / duration);
                }
                else
                {
                    _image.fillAmount = Mathf.Lerp(end, start, elapsed / duration);
                }

                yield return null;
            }
        }

        private IEnumerator CountDown(float duration, bool isRealTime)
        {
            var elapsed = duration + 1;
            while (elapsed > 0)
            {
                elapsed = elapsed - 1;
                _text.text = elapsed.ToString();
                if (isRealTime)
                {
                    yield return new WaitForSecondsRealtime(1f);
                }
                else
                {
                    yield return new WaitForSeconds(1f);
                }
            }
        }

        public Volume GetVolume()
        {
            return _specialUIHandler.GetVolume();
        }

        public void SetVolumeProfile(VolumeProfile volumeProfile = null, float weight = 1f)
        {
            _specialUIHandler.SetVolumeProfile(volumeProfile, weight);
        }
    }
}

