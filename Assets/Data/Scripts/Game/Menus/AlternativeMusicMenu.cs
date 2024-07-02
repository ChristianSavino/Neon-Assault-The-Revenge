using Keru.Scripts.Engine;
using Keru.Scripts.Engine.Master;
using Keru.Scripts.Engine.Module;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Keru.Scripts.Game.Menus
{
    public class AlternativeMusicMenu : MonoBehaviour
    {
        [SerializeField] private WeaponSelectionMenu _weaponSelectionMenu;
        [SerializeField] private List<MusicData> _originalSongs;
        [SerializeField] private List<MusicData> _alternateSongs;
        [SerializeField] Text _levelNameText;

        private string _levelName;

        private void Start()
        {
            _levelNameText.text = $"{_levelName} -> selecciona música".ToUpper();
            JukeBox.jukebox.StopMusic(true);
        }

        public void SetLevelName(string levelName)
        {
            _levelName = levelName;
        }

        public void SelectSong(bool alternate)
        {
            var saveGame = LevelBase.CurrentSave;
            saveGame.AlternateMusic = alternate;
            SaveManager.saveManager.SaveGame(saveGame);
            LevelSceneManager.levelSceneManager.LoadScene(saveGame.CurrentLevelCode);
        }

        public void GoBack()
        {
            _weaponSelectionMenu.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }

        public MusicData GetCurrentSong(bool isAlternate, LevelCode levelCode)
        {
            if (isAlternate)
            {
                return _alternateSongs.First(x => x.LevelCode == levelCode);
            }

            return _originalSongs.First(x => x.LevelCode == levelCode);
        }
    }

    [Serializable]
    public class MusicData
    {
        public LevelCode LevelCode;
        public string SongName;
        public AudioClip SongClip;
    }
}