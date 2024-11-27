using Keru.Scripts.Engine;
using Keru.Scripts.Engine.Master;
using Keru.Scripts.Engine.Module;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Keru.Scripts.Game.Menus.GameMainMenu.MusicSelection
{
    public class SoundSelectionObject : MonoBehaviour
    {
        [SerializeField] private Text _songName;
        [SerializeField] private Text _songArtist;
        [SerializeField] private Image _soundIcon;

        [SerializeField] private List<Sprite> _soundIcons;
        private AudioSource _soundSource;
        
        public void SetUp(SongSheet songSheet)
        {
            _songArtist.text = songSheet.Artist.ToUpper();
            _songName.text = songSheet.Name.ToUpper();
            _soundIcon.sprite = _soundIcons[0];

            _soundSource = AudioManager.audioManager.CreateNewAudioSource(gameObject, SoundType.Music);
            _soundSource.clip = songSheet.Audio;
            _soundSource.loop = true;
        }

        public void Play()
        {
            _soundIcon.sprite = _soundIcons[1];
            _soundSource.Play();
            JukeBox.jukebox.StopMusic(false);
        }

        public void Stop()
        {
            _soundIcon.sprite = _soundIcons[0];
            _soundSource.Stop();
            JukeBox.jukebox.ChangeSong(MusicTracks.Ambience, false);
        }
    }
}