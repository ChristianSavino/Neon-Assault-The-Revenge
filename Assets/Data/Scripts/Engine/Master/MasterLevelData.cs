using System.Collections.Generic;

namespace Keru.Scripts.Engine.Master
{
    public static class MasterLevelData
    {
        public static List<LevelData> AllLevels { get; } = new List<LevelData>()
    {
        new LevelData()
        {
            Code = LevelCode.Level0,
            LevelName = "Prólogo",
            Description = "Volver a ver la cinemática inicial",
            LevelType = LevelType.Cutscene,
            NextLevel = LevelCode.GameMainMenu,
            SceneName = "Prologue",
            Unlocks = new List<LevelCode>()
            {
                LevelCode.Level1
            },
            HasPredefinedCharacters = false,
            IsPreviousLevelCutscene = false,
        },
        new LevelData()
        {
            Code = LevelCode.Level1,
            LevelName = "Misión 1 - Club Babylon",
            Description = "La nota indica que debo ir a este club, no sé quién la ha escrito, pero tengo el presentimiento que debo ir con la guardia alta.",
            LevelType = LevelType.Game,
            NextLevel = LevelCode.GameMainMenu,
            SceneName = "Level1",
            Unlocks = new List<LevelCode>()
            {
                LevelCode.Level2,
                LevelCode.Special1
            }
        },
        new LevelData()
        {
            Code = LevelCode.Special1,
            LevelName = "El Futuro en el Pasado - Parte 1",
            LevelType = LevelType.Game,
            NextLevel = LevelCode.GameMainMenu,
            SceneName = "Special1",
            Unlocks = new List<LevelCode>()
        }
    };
    }

    public class LevelData
    {
        public LevelCode Code { get; set; }
        public string LevelName { get; set; }
        public string Description { get; set; }
        public string SceneName { get; set; }
        public LevelType LevelType { get; set; }
        public List<LevelCode> Unlocks { get; set; }
        public LevelCode NextLevel { get; set; }
        public bool HasPredefinedCharacters { get; set; }
        public bool IsPreviousLevelCutscene { get; set; }
    }
}