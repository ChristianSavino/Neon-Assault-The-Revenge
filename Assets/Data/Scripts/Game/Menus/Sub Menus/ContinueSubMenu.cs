using Keru.Scripts.Engine.FileSystem;
using Keru.Scripts.Engine.Master;
using Keru.Scripts.Engine.Module;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Keru.Scripts.Game.Menus.SubMenus
{
    public class ContinueSubMenu : MonoBehaviour
    {
        [SerializeField] private List<Button> _saveSlots;

        private void OnEnable()
        {
            LoadAllSaveFiles();
        }

        private void LoadAllSaveFiles()
        {
            var saveFiles = ExternalFilesManager.LoadAllSavedGames();

            for (int i = 0; i < _saveSlots.Count; i++)
            {
                var saveFile = saveFiles[i];
                var saveSlotText = _saveSlots[i].GetComponentInChildren<Text>();

                if (saveFile != null)
                {
                    var chapter = MasterLevelData.AllLevels.FirstOrDefault(x => x.Code == saveFile.CurrentLevelCode);
                    saveSlotText.text = $"{saveFile.SavePosition + 1} - {(chapter != null ? chapter.LevelName.ToUpper() : "Selección de misión".ToUpper())} - {saveFile.LastSaveDate.ToString("dd/MM/yyyy HH:mm:ss")}";
                }
                else
                {
                    _saveSlots[i].interactable = false;
                    saveSlotText.color = _saveSlots[i].colors.disabledColor;
                }
            }
        }

        public void LoadGame(int saveGameLocation)
        {
            DisableAllButtons();

            LevelBase.levelBase.LoadGame(saveGameLocation);
        }

        private void DisableAllButtons()
        {
            foreach (var button in _saveSlots)
            {
                button.interactable = false;
            }
        }
    }
}