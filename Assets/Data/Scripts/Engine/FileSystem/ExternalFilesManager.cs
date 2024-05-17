using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

namespace Keru.Scripts.Engine.FileSystem
{
    public static class ExternalFilesManager
    {
        public const string MainFileName = "Main";
        public const string SaveFileName = "SavedGame";

        public static MainGameData LoadGameData()
        {
            var dataJson = FileSystem.LoadFile(MainFileName);
            if (string.IsNullOrEmpty(dataJson))
            {
                FileSystem.DeleteOldVersionFiles();
                var mainGameData = new MainGameData();
                FileSystem.SaveFile(mainGameData, MainFileName);
                return mainGameData;
            }
            else
            {
                var gameData = JsonConvert.DeserializeObject<MainGameData>(dataJson);
                if(gameData.Version != Application.version)
                {
                    var mainGameData = new MainGameData();
                    mainGameData.Options = gameData.Options;
                    gameData = mainGameData;
                }

                return gameData;
            }
        }

        public static void UpdateGameData(MainGameData data)
        {
            FileSystem.SaveFile(data, MainFileName);
        }

        #region SaveData

        public static List<SaveGameFile> LoadAllSavedGames()
        {
            int i = 0;
            List<SaveGameFile> savedGames = new List<SaveGameFile>();
            for (i = 0; i < 6; i++)
            {
                savedGames.Add(LoadSavedGame(i));
            }

            return savedGames;
        }

        public static SaveGameFile LoadSavedGame(int i)
        {
            var dataJson = FileSystem.LoadFile($"{SaveFileName}{i}");
            return JsonConvert.DeserializeObject<SaveGameFile>(dataJson);
        }

        public static void SaveGame(SaveGameFile ps)
        {
            FileSystem.SaveFile(ps, $"{SaveFileName}{ps.SavePosition}");
        }

        #endregion
    }
}