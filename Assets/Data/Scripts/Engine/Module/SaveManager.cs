using Keru.Scripts.Engine.FileSystem;
using UnityEngine;

namespace Keru.Scripts.Engine.Module
{
    public class SaveManager : MonoBehaviour
    {
        public static SaveManager saveManager;

        public void SetUp()
        {
            saveManager = this;
        }

        public SaveGameFile LoadSaveGame(int savePosition)
        {
            return ExternalFilesManager.LoadSavedGame(savePosition);
        }

        public void SaveGame(SaveGameFile saveGameFile)
        {
            ExternalFilesManager.SaveGame(saveGameFile);
        }

        public SaveGameFile CreateNewSaveGame(int savePosition, int difficulty)
        {
            var saveGame = new SaveGameFile(savePosition);
            saveGame.Difficulty = (Difficulty)difficulty;
            ExternalFilesManager.SaveGame(saveGame);

            return saveGame;
        }
    }
}