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
        
        private SaveGameFile _savedGame;
        private string _selectedName;
        private Dictionary<string, List<string>> _levelData;

        private void Start()
        {
            _savedGame = LevelBase.CurrentSave;
            Cancel();
            CreateLevelData();
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
            var levelData = _levelData[_selectedName];

            _levelName.text = levelData[0].ToUpper();
            _levelDescription.text = levelData[1].ToUpper();

            ToggleOffAllButtons();
            ToggleSelectedLevelData(true);
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

        private void CreateLevelData()
        {
            _levelData = new Dictionary<string, List<string>>()
            {
                {
                    "Prologue", 
                    new List<string>()
                    {
                        "Prólogo - 1998",
                        "El Comienzo"
                    } 
                },
                {
                    "Level1",
                    new List<string>()
                    {
                        "Capítulo 1 - Club Nocturno",
                        "La nota indica que debemos\nir a la zona indicada,\nel Club Babylon\n\nnuestro objetivo es encontrar\nal contacto para obtener información\n sobre que nos pasó"
                    }
                },
                {
                    "Special1",
                    new List<string>()
                    {
                        "Especial 1 - Pasado Al Futuro",
                        "El punto apareció misteriosamente,\nprobablemente sea una trampa\npero vale la pena investigar"
                    }
                }
            };
        }
    }
}