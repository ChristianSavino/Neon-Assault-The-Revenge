using Keru.Scripts.Engine.Master;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Keru.Scripts.Engine.Module
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager audioManager;

        private List<AudioSource> _effects;
        private List<AudioSource> _music;
        private List<AudioSource> _voices;

        public void SetUp()
        {
            audioManager = this;

            var audioOptions = LevelBase.GameOptions.Options.AudioOptions;

            var audioConfig = AudioSettings.GetConfiguration();
            audioConfig.speakerMode = audioOptions.Surround != 0 ? (AudioSpeakerMode)audioOptions.Surround : AudioSpeakerMode.Mono;
            AudioSettings.Reset(audioConfig);

            _effects = new List<AudioSource>();
            _music = new List<AudioSource>();
            _voices = new List<AudioSource>();
        }

        public AudioSource CreateNewAudioSource(GameObject origin, SoundType soundType)
        {
            var audioOptions = LevelBase.GameOptions.Options.AudioOptions;
            var audioSource = origin.AddComponent<AudioSource>();
            audioSource.loop = false;
            audioSource.playOnAwake = false;

            switch (soundType)
            {
                case SoundType.Effect:
                    audioSource.volume = audioOptions.EffectVolume;
                    _effects.Add(audioSource);
                    break;
                case SoundType.Music:
                    audioSource.volume = audioOptions.MusicVolume;
                    _music.Add(audioSource);
                    break;
                case SoundType.Voice:
                    audioSource.volume = audioOptions.VoiceVolume;
                    _voices.Add(audioSource);
                    break;
            }

            return audioSource;
        }

        public void SetNewAudioValues()
        {
            _effects = CleanNullAudioSources(_effects);
            _music = CleanNullAudioSources(_music);
            _voices = CleanNullAudioSources(_voices);

            var audioOptions = LevelBase.GameOptions.Options.AudioOptions;

            EstablishSoundPerList(_effects, audioOptions.EffectVolume);
            EstablishSoundPerList(_music, audioOptions.MusicVolume);
            EstablishSoundPerList(_voices, audioOptions.VoiceVolume);
        }

        private void EstablishSoundPerList(List<AudioSource> sounds, float soundVolume)
        {
            foreach (AudioSource audioSource in sounds)
            {
                audioSource.volume = soundVolume;
            }
        }

        public void SetPitch(float pitch)
        {
            _effects = CleanNullAudioSources(_effects);
            _music = CleanNullAudioSources(_music);
            _voices = CleanNullAudioSources(_voices);

            EstablishPitchPerList(_effects, pitch);
            EstablishPitchPerList(_music, pitch);
            EstablishPitchPerList(_voices, pitch);
        }

        private void EstablishPitchPerList(List<AudioSource> sounds, float pitch)
        {
            foreach (var audioSource in sounds)
            {
                audioSource.pitch = pitch;
            }
        }

        private List<AudioSource> CleanNullAudioSources(List<AudioSource> sounds)
        {
            return sounds.Where(s => s != null).ToList();
        }
    }
}