using Keru.Scripts.Engine;
using Keru.Scripts.Engine.Module;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Keru.Debug.Scripts.Game.Entities.Player.UI
{
    public class DashUIHandler : MonoBehaviour
    {
        [SerializeField] private Image _dashAmmount;
        [SerializeField] private Image _dashRecover;
        [SerializeField] private AudioClip _dashRecoverSound;

        private int _maxDashes;
        private float _ammountPerCharge;
        private float _refreshTime;
        private Coroutine _refreshRoutine;

        private AudioSource _audioSource;

        public void SetUp(int maxDashes, float refreshTime)
        {
            _maxDashes = maxDashes;
            _refreshTime = refreshTime;
            _ammountPerCharge = 1f / _maxDashes;
            _dashAmmount.fillAmount = 1;
            _dashRecover.fillAmount = 1;
            _audioSource = AudioManager.audioManager.CreateNewAudioSource(gameObject, SoundType.Effect);
        }

        public void UpdateBars(int dashes)
        {
            if (_refreshRoutine != null)
            {
                StopCoroutine(_refreshRoutine);
            }
            var currentDashesFill = _ammountPerCharge * dashes;
            _dashAmmount.fillAmount = currentDashesFill;
            _dashRecover.fillAmount = currentDashesFill;
            if (dashes != _maxDashes)
            {
                if (enabled)
                {
                    _refreshRoutine = StartCoroutine(RecoverDash(dashes));
                }
            }

        }

        private IEnumerator RecoverDash(int dashes)
        {
            var start = _dashRecover.fillAmount;
            var end = _ammountPerCharge * (dashes + 1);
            var elapsed = 0f;

            while (elapsed < _refreshTime)
            {
                elapsed += Time.deltaTime;
                _dashRecover.fillAmount = Mathf.Lerp(start, end, elapsed / _refreshTime);
                yield return null;
            }
            _dashAmmount.fillAmount = end;
            _audioSource.PlayOneShot(_dashRecoverSound);
        }


        public void Die()
        {
            if (_refreshRoutine != null)
            {
                StopCoroutine(_refreshRoutine);
            }
            enabled = false;
        }
    }
}
