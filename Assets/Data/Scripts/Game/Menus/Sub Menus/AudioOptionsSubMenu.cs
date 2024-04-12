using Keru.Scripts.Engine.FileSystem;
using Keru.Scripts.Engine.Master;
using Keru.Scripts.Engine.Module;
using UnityEngine;
using UnityEngine.UI;

namespace Keru.Scripts.Game.Menus.SubMenus
{
    public class AudioOptionsSubMenu : MonoBehaviour
    {
        private AudioOptions _audioOptions;

        [Header("Effects")]
        [SerializeField] private Slider _effectsQuantity;
        [SerializeField] private Text _effectsAmmount;

        [Header("Music")]
        [SerializeField] private Slider _musicQuantity;
        [SerializeField] private Text _musicAmmount;

        [Header("Voices")]
        [SerializeField] private Slider _voicesQuantity;
        [SerializeField] private Text _voicesAmmount;

        [Header("Surround")]
        [SerializeField] private Slider _surroundQuantity;
        [SerializeField] private Text _surroundAmmount;

        private void OnEnable()
        {
            _audioOptions = LevelBase.GameOptions.Options.AudioOptions;

            _effectsQuantity.value = _audioOptions.EffectVolume * 100f;
            _musicQuantity.value = _audioOptions.MusicVolume * 100f;
            _voicesQuantity.value = _audioOptions.VoiceVolume * 100f;
            _surroundQuantity.value = _audioOptions.Surround;

            UpdateEffectsVolume();
            UpdateMusicVolume();
            UpdateVoiceVolume();
            UpdateSurround();
        }

        public void SaveChanges()
        {
            ExternalFilesManager.UpdateGameData(LevelBase.GameOptions);
            MenuConsole.menuConsole.Message("Audio configurado");
            AudioManager.audioManager.SetNewAudioValues();
            gameObject.SetActive(false);
        }

        public void UpdateEffectsVolume()
        {
            _audioOptions.EffectVolume = _effectsQuantity.value / 100f;
            _effectsAmmount.text = $"{_effectsQuantity.value}%";
        }

        public void UpdateMusicVolume()
        {
            _audioOptions.MusicVolume = _musicQuantity.value / 100f;
            _musicAmmount.text = $"{_musicQuantity.value}%";
        }

        public void UpdateVoiceVolume()
        {
            _audioOptions.VoiceVolume = _voicesQuantity.value / 100f;
            _voicesAmmount.text = $"{_voicesQuantity.value}%";
        }

        public void UpdateSurround()
        {
            _audioOptions.Surround = (int)_surroundQuantity.value;
            var textToSet = "";
            switch(_audioOptions.Surround)
            {
                case 1:
                    textToSet = "MONO";
                    break;
                case 2:
                    textToSet = "STEREO";
                    break;
                case 3:
                    textToSet = "QUAD";
                    break;
                case 4:
                    textToSet = "SURROUND";
                    break;
                case 5:
                    textToSet = "5.1";
                    break;
                case 6:
                    textToSet = "7.1";
                    break;
                case 7:
                    textToSet = "PROLOGIC";
                    break;
            }

            _surroundAmmount.text = textToSet;
        }
    }
}