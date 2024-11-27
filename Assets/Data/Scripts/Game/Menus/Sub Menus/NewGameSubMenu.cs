using Keru.Scripts.Engine;
using Keru.Scripts.Engine.FileSystem;
using Keru.Scripts.Engine.Master;
using Keru.Scripts.Engine.Module;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Keru.Scripts.Game.Menus.SubMenus
{
    public class NewGameSubMenu : MonoBehaviour
    {
        [SerializeField] private List<Button> _saveSlots;
        [SerializeField] private GameObject _askMesssage;
        [SerializeField] private Text _difficultyText;

        private int _saveGameSlot;
        private List<SaveGameFile> _saveFiles;
        private int _difficulty;

        private void OnEnable()
        {
            _askMesssage.SetActive(false);
            LoadAllSaveFiles();
            ChangeDiff(0);
        }

        private void LoadAllSaveFiles()
        {
            _saveFiles = ExternalFilesManager.LoadAllSavedGames();

            for (int i = 0; i < _saveSlots.Count; i++)
            {
                var saveFile = _saveFiles[i];

                if (saveFile != null)
                {
                    var chapter = MasterLevelData.AllLevels.FirstOrDefault(x => x.Code == saveFile.CurrentLevelCode);
                    _saveSlots[i].GetComponentInChildren<Text>().text = $"{saveFile.SavePosition+1} - {(chapter != null ? chapter.LevelName.ToUpper() : "Selección de misión".ToUpper())} - {saveFile.LastSaveDate.ToString("dd/MM/yyyy HH:mm:ss")}";
                }
            }
        }

        public void NewGame(int saveGameSlot)
        {
            _saveGameSlot = saveGameSlot;
            
            if(_saveFiles[saveGameSlot] != null)
            {
                _askMesssage.SetActive(true);
            }
            else
            {
                Yes();
            }
        }

        public void Yes()
        {
            _askMesssage.SetActive(false);

            DisableAllButtons();

            var saveGame = LevelBase.levelBase.CreateNewSaveGame(_saveGameSlot, _difficulty);         
            LevelSceneManager.levelSceneManager.LoadScene(saveGame.CurrentLevelCode);
        }

        public void No()
        {
            _askMesssage.SetActive(false);
        }

        private void DisableAllButtons()
        {
            foreach(var button in _saveSlots)
            {
                button.interactable = false;
            }
        }

        public void ChangeDiff(int direction)
        {
            _difficulty += direction;
            if(_difficulty > (int)Difficulty.KeruMustDie)
            {
                _difficulty = 0;
            }
            var text = "";

            switch (_difficulty)
            {
                case 0:
                    text = "Fácil";
                    break;
                case 1:
                    text = "Normal";
                    break;
                case 2:
                    text = "Difícil";
                    break;
                case 3:
                    text = "Keru debe Morir";
                    break;
            }

            _difficultyText.text = text.ToUpper();
        }
    }
}