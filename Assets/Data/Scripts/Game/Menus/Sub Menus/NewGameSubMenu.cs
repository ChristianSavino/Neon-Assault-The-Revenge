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

        private int _saveGameSlot;
        private List<SaveGameFile> _saveFiles;

        private void OnEnable()
        {
            _askMesssage.SetActive(false);
            LoadAllSaveFiles();
        }

        private void LoadAllSaveFiles()
        {
            _saveFiles = ExternalFilesManager.LoadAllSavedGames();

            for (int i = 0; i < _saveSlots.Count; i++)
            {
                var saveFile = _saveFiles[i];

                if (saveFile != null)
                {
                    var chapter = saveFile.AllLevelData.Where(x => x.Code == saveFile.CurrentLevelCode).FirstOrDefault();
                    _saveSlots[i].GetComponentInChildren<Text>().text = $"{saveFile.SavePosition+1} - {(chapter != null ? chapter.LevelName.ToUpper() : "Seleccion de mision".ToUpper())} - {saveFile.LastSaveDate.ToString("dd/MM/yyyy HH:mm:ss")}";
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

            var saveGame = LevelBase.levelBase.CreateNewSaveGame(_saveGameSlot);         
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
    }
}