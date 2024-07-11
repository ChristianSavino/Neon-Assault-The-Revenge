using Keru.Scripts.Engine;
using Keru.Scripts.Engine.Master;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Keru.Scripts.Game.Menus
{
    public class MapMainMenu : MonoBehaviour
    {
        [SerializeField] private List<Button> _buttons;
        [SerializeField] private GameObject _selectedLevelData;
        [SerializeField] private Text _levelName;
        [SerializeField] private Text _levelDescription;
        [SerializeField] private CharacterSelectionMenu _characterSelection;
        [SerializeField] private Text _diffText;
        
        private SaveGameFile _savedGame;
        private string _selectedName;
        private int _difficulty;

        private void Start()
        {
            _savedGame = LevelBase.CurrentSave;
            _difficulty = (int)_savedGame.Difficulty;
            Cancel();
            LoadCorrectDiff();
        }

        private void ToggleOffAllButtons()
        {
            foreach(var button in _buttons)
            {
                button.gameObject.SetActive(false);
            }
        }

        private void ToggleOnButtons()
        {
            ToggleOffAllButtons();

            foreach (var levelSavedData in _savedGame.AllLevelData)
            {
                var levelData = MasterLevelData.AllLevels.First(x => x.Code == levelSavedData.Code);
                
                var button = _buttons.Find(x => x.gameObject.name == levelData.SceneName);
                button.gameObject.SetActive(true);
                button.transform.Find("Completed").gameObject.SetActive(levelSavedData.Completed);
            }
        }

        public void LevelPick(GameObject button)
        {
            _selectedName = button.gameObject.name;
            var levelData = MasterLevelData.AllLevels.First(x => x.SceneName == _selectedName);

            _levelName.text = levelData.LevelName.ToUpper();
            _levelDescription.text = levelData.ShortDescription.ToUpper();

            ToggleOffAllButtons();
            ToggleSelectedLevelData(true);
        }

        public void ChangeDifficulty()
        {
            _difficulty++;
            if(_difficulty > 3)
            {
                _difficulty = 0;
            }
            var difficulty = (Difficulty)_difficulty;

            _savedGame.Difficulty = difficulty;
            LoadCorrectDiff();
        }

        private void LoadCorrectDiff()
        {
            switch (_savedGame.Difficulty)
            {
                case Difficulty.Easy:
                    _diffText.text = "FÁCIL";
                    break;
                case Difficulty.Normal:
                    _diffText.text = "NORMAL";
                    break;
                case Difficulty.Hard:
                    _diffText.text = "DIFÍCIL";
                    break;
                case Difficulty.KeruMustDie:
                    _diffText.text = "KERU DEBE MORIR";
                    break;
            }
        }

        public void Continue()
        {
            var levelCode = (LevelCode)Enum.Parse(typeof(LevelCode), _selectedName);
            var currentLevel = MasterLevelData.AllLevels.FirstOrDefault(x => x.Code == levelCode);
            var previousCutsceneLevel = MasterLevelData.AllLevels.FirstOrDefault(x => x.NextLevel == levelCode && x.IsPreviousLevelCutscene);
            if (previousCutsceneLevel != null) 
            {
                levelCode = previousCutsceneLevel.Code;
            }
            
            _savedGame.CurrentLevelCode = levelCode;

            if(!currentLevel.HasPredefinedCharacters)
            {
                _characterSelection.SetLevelName(_levelName.text);
                _characterSelection.gameObject.SetActive(true);
                ToggleSelectedLevelData(false);

                ToggleOnButtons();

                gameObject.SetActive(false);
            }
            else
            {
                LevelBase.levelBase.LoadSelectedLevel();
            }
        }

        public void Cancel()
        {
            ToggleSelectedLevelData(false);
            ToggleOnButtons();
        }

        private void ToggleSelectedLevelData(bool toggle)
        {
            _selectedLevelData.SetActive(toggle);
        }
    }
}