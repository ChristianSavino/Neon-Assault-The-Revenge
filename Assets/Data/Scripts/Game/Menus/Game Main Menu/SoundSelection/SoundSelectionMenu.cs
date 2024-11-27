using Keru.Scripts.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Keru.Scripts.Game.Menus.GameMainMenu.MusicSelection
{
    public class SoundSelectionMenu : MonoBehaviour
    {
        [Header("Objects")]
        [SerializeField] private GameObject _cantStartLevel;
        [SerializeField] private GameObject _menuSong;

        [Header("Song Data")]
        [SerializeField] private SoundSelectionObject _originalSong;
        [SerializeField] private SoundSelectionObject _alternateSong;
        [SerializeField] private List<SoundSelectionData> _songs;

        private LevelCode? _level;
        private GameMainMenuData _gameMainMenuData;

        private void OnEnable()
        {
            if(_gameMainMenuData == null )
            {
                _gameMainMenuData = transform.parent.parent.GetComponentInChildren<GameMainMenuData>();
            }
            if(_gameMainMenuData.GetLevelCode() == null)
            {
                _cantStartLevel.SetActive(true);
                _menuSong.SetActive(false);
            }
            else
            {
                if(_gameMainMenuData.GetLevelCode() == LevelCode.Level0)
                {
                    _cantStartLevel.SetActive(false);
                    _menuSong.SetActive(false);
                    _gameMainMenuData.PrepareDataForNextLevel(false);
                }
                else
                {
                    _cantStartLevel.SetActive(false);
                    _menuSong.SetActive(true);

                    var soundSelectionData = _songs.Where(x => x.LevelCode == _gameMainMenuData.GetLevelCode()).First();
                    _originalSong.SetUp(soundSelectionData.OriginalSong);
                    _alternateSong.SetUp(soundSelectionData.AlternateSong);
                }
            }
        }

        public void SongChosen(bool isAlternate)
        {
            gameObject.SetActive(false);
            _gameMainMenuData.PrepareDataForNextLevel(isAlternate);
        }
    }

    [Serializable]
    public class SoundSelectionData
    {
        public LevelCode LevelCode;
        public SongSheet OriginalSong;
        public SongSheet AlternateSong;
    }

    [Serializable]
    public class SongSheet
    {
        public string Name;
        public string Artist;
        public AudioClip Audio;
    }
}