using Keru.Scripts.Engine;
using Keru.Scripts.Engine.Module;
using UnityEngine;

namespace Keru.Scripts.Game.Entities.Player
{
    public class PlayerEffectsHandler : MonoBehaviour
    {
        private AudioSource _audioSource;

        public void SetConfig()
        {
            _audioSource = AudioManager.audioManager.CreateNewAudioSource(gameObject, SoundType.Effect);
        }

        public void CreateEffect(AudioClip soundEffect = null, GameObject effect = null, float? overrideDuration = null)
        {
            if (soundEffect != null)
            {
                _audioSource.PlayOneShot(soundEffect);
            }

            if (effect != null)
            {
                var effectInstance = Instantiate(effect, transform.position, Quaternion.identity, transform);
                if (overrideDuration.HasValue)
                {
                    Destroy(effectInstance, overrideDuration.Value);
                }
            }
        }
    }
}