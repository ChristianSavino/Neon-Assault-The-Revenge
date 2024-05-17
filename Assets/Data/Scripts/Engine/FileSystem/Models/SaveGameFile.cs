using Keru.Scripts.Engine;
using System;
using System.Collections.Generic;

[System.Serializable]
public class SaveGameFile
{
    public int SavePosition { get; set; }
    public int Checkpoint { get; set; }
    public LevelCode CurrentLevelCode { get; set; }
    public DateTime LastSaveDate { get; set; }
    public List<LevelData> AllLevelData { get; set; }

    public SaveGameFile(int savePosition)
    {
        SavePosition = savePosition;
        Checkpoint = 0;
        CurrentLevelCode = LevelCode.Level0;
        LastSaveDate = DateTime.Now;
        AllLevelData = new List<LevelData>()
        {
            new LevelData()
            {
                Code = LevelCode.Level0,
                LevelName = "Prólogo",
                LevelType = LevelType.Cutscene,
                NextLevel = LevelCode.GameMainMenu,
                SceneName = "Prologue",
                Unlocked = true,
                Unlocks = LevelCode.Level1
            },
            new LevelData()
            {
                Code = LevelCode.Level1,
                LevelName = "Misión 1 - Club Nocturno",
                LevelType = LevelType.Game,
                NextLevel = LevelCode.GameMainMenu,
                SceneName = "Level1",
                Unlocked = false,
                Unlocks = LevelCode.Level2
            }
        };
    }
}
