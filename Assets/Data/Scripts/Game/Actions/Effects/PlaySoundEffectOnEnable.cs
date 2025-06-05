using Keru.Scripts.Engine;
using Keru.Scripts.Engine.Module;
using System.Collections;
using UnityEngine;

namespace Keru.Game.Actions.Effects
{
    public class PlaySoundEffectOnEnable : Action
    {
        [SerializeField] private AudioClip _soundEffect;
        [SerializeField] private SoundType _soundType;
        [SerializeField] private float _pitch = 1f;
        [SerializeField] private bool _loop = false;
        [SerializeField] private float _delay = 0.1f;

        private AudioSource _audioSource;

        private void OnEnable()
        {
            Execute();
        }

        public override void Execute()
        {
            _audioSource = AudioManager.audioManager.CreateNewAudioSource(gameObject, _soundType);
            _audioSource.clip = _soundEffect;

            if (_delay > 0f)
            {
               StartCoroutine(PlayDelayed());
            }
            else
            {
                PlaySound();
            }

            
        }

        private IEnumerator PlayDelayed()
        {
            yield return new WaitForSecondsRealtime(_delay);
            PlaySound();
        }

        private void PlaySound()
        {
            _audioSource.pitch = _pitch;
            if (_loop)
            {
                _audioSource.loop = true;
                _audioSource.Play();
            }
            else
            {
                _audioSource.PlayOneShot(_soundEffect);
            }
        }
    }
}