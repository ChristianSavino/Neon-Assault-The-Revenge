using Keru.Scripts.Engine.FileSystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Keru.Scripts.Game.Menus.SubMenus
{
    public class NewGameSubMenu : MonoBehaviour
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
                if (saveFiles[i] != null)
                {
                    _saveSlots[i].GetComponentInChildren<Text>().text = $"Hola";
                }
            }
        }

        public void NewGame(int saveGameSlot)
        {
            print($"New Game Created -> {saveGameSlot}");
        }
    }
}