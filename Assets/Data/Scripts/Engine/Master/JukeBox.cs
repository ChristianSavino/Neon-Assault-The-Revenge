using Keru.Scripts.Engine.Module;
using System.Collections;
using UnityEngine;

namespace Keru.Scripts.Engine.Master
{
    public class JukeBox : MonoBehaviour
    {
        public static JukeBox jukebox;

        [Header("Original Music")]
        [SerializeField] private AudioClip _ambienceMusic;
        [SerializeField] private AudioClip _assaultMusic;
        [SerializeField] private AudioClip _bossMusic;

        [Header("Alternate Music")]
        [SerializeField] private AudioClip _alternateAmbienceMusic;
        [SerializeField] private AudioClip _alternateAssaultMusic;
        [SerializeField] private AudioClip _alternateBossMusic;

        private AudioSource _musicSource;
        private AudioSource _auxMusicSource;
        private bool _usesAlternateMusic;

        public void SetUp(bool usesAlternateMusic)
        {
            jukebox = this;
            _musicSource = AudioManager.audioManager.CreateNewAudioSource(gameObject, SoundType.Music);
            _auxMusicSource = AudioManager.audioManager.CreateNewAudioSource(gameObject, SoundType.Music);
            _usesAlternateMusic = usesAlternateMusic;
        }

        public void Pause(bool isPaused)
        {
            if (isPaused)
            {
                _musicSource.Pause();
                if (_auxMusicSource.isPlaying)
                {
                    _auxMusicSource.Pause();
                }
            }
            else
            {
                _auxMusicSource.UnPause();
                _musicSource.UnPause();
            }
        }

        public void AudioToggle(bool on)
        {
            if (on)
            {
                _musicSource.Play();
            }
            else
            {
                _musicSource.Stop();
            }
        }

        private void SelectSong(MusicTracks track)
        {
            if (_musicSource.isPlaying)
            {
                _musicSource.Stop();
            }

            switch (track)
            {
                case MusicTracks.Ambience:
                    _musicSource.clip = !_usesAlternateMusic ? _ambienceMusic : _alternateAmbienceMusic;
                    break;
                case MusicTracks.Assault:
                    _musicSource.clip = !_usesAlternateMusic ? _assaultMusic : _alternateAssaultMusic;
                    break;
                case MusicTracks.Boss:
                    _musicSource.clip = !_usesAlternateMusic ? _bossMusic : _alternateBossMusic;
                    break;
            }
            
            _musicSource.Play();
        }

        public void ChangeSong(MusicTracks track, bool smoothChange)
        {
            if (smoothChange)
            {
                StartCoroutine(SmoothChangeSong(track));
            }
            else
            {
                SelectSong(track);
            }
        }

        private IEnumerator SmoothChangeSong(MusicTracks track)
        {
            var musicVolume = _musicSource.volume;

            _auxMusicSource.clip = _musicSource.clip;
            _auxMusicSource.time = _musicSource.time;
            _auxMusicSource.volume = musicVolume;

            _auxMusicSource.Play();
            SelectSong(track);
            _musicSource.volume = 0;

            while (_musicSource.volume < musicVolume)
            {
                yield return new WaitForSeconds(Time.deltaTime / 2);
                var ammountToSum = musicVolume * (Time.deltaTime / 2);
                _musicSource.volume += ammountToSum;
                _auxMusicSource.volume -= ammountToSum;
            }

            _musicSource.volume = musicVolume;
            _auxMusicSource.Stop();
        }

        public void StopMusic(bool smooth)
        {
            if(smooth)
            {
                StartCoroutine(SmoothStop());
            }
            else
            {
                _musicSource.Stop();
            }
        }

        private IEnumerator SmoothStop()
        {
            var musicVolume = _musicSource.volume;
            while (_musicSource.volume > 0)
            {
                yield return new WaitForSeconds(Time.deltaTime / 2);
                var ammountToSum = musicVolume * (Time.deltaTime / 2);
                _musicSource.volume -= ammountToSum;
            }
        }
    }
}
