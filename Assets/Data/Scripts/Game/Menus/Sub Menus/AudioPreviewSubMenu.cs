using Keru.Scripts.Engine;
using Keru.Scripts.Engine.Master;
using Keru.Scripts.Engine.Module;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Keru.Scripts.Game.Menus.SubMenus
{
    public class AudioPreviewSubMenu : MonoBehaviour
    {
        private AudioSource _audioSource;
        private float _musicVolume;

        [SerializeField] private Text _songName;
        [SerializeField] private Image _soundIcon;
        [SerializeField] private Sprite _playIcon;
        [SerializeField] private Sprite _stopIcon;
        [SerializeField] private bool _isAlternateMusic;

        private void Start()
        {
            var musicData = transform.GetComponentInParent<AlternativeMusicMenu>().GetCurrentSong(_isAlternateMusic, LevelBase.CurrentSave.CurrentLevelCode);
            
            _audioSource = AudioManager.audioManager.CreateNewAudioSource(gameObject, SoundType.Music);
            _audioSource.clip = musicData.SongClip;
            _audioSource.loop = true;
            _musicVolume = _audioSource.volume;

            _songName.text = musicData.SongName.ToUpper();
            _soundIcon.sprite = _stopIcon;
        }

        public void PlaySong()
        {           
            StopCoroutine(SmoothStop());
            _audioSource.volume = _musicVolume;
            _audioSource.Play(); 
            _soundIcon.sprite = _playIcon;
        }

        public void StopSong()
        {
            StartCoroutine(SmoothStop());
        }
        private IEnumerator SmoothStop()
        {         
            while (_audioSource.volume > 0)
            {
                yield return new WaitForSeconds(Time.deltaTime*2);
                var ammountToSum = _musicVolume * (Time.deltaTime*2);
                _audioSource.volume -= ammountToSum;
            }

            _audioSource.Stop();
            _soundIcon.sprite = _stopIcon;
        }
    }
}
