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
            Code = LevelCode.Level1PreCutscene,
            LevelName = "Misión 1 - Cinematica Inicial",
            LevelType = LevelType.Cutscene,
            NextLevel = LevelCode.Level1,
            HasPredefinedCharacters = false,
            IsPreviousLevelCutscene = true,
            SceneName = "Level1Cut",
            Unlocks =new List<LevelCode>()
        },
        new LevelData()
        {
            Code = LevelCode.Level1,
            LevelName = "Misión 1 - Club Nocturno",
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
        public string SceneName { get; set; }
        public LevelType LevelType { get; set; }
        public List<LevelCode> Unlocks { get; set; }
        public LevelCode NextLevel { get; set; }
        public bool HasPredefinedCharacters { get; set; }
        public bool IsPreviousLevelCutscene { get; set; }
    }
}