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
            LevelName = "Pr�logo - 1998",
            ShortDescription = "El Comienzo",
            Description = "El Comienzo",
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
            LevelName = "Misi�n 1 - Cinematica Inicial",
            Description = "",
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
            LevelName = "Misi�n 1 - Club Nocturno",
            ShortDescription = "La nota indica que debemos\nir a la zona indicada,\nun club nocturno\n\nnuestro objetivo es encontrar\nal contacto para obtener informaci�n\n sobre que nos pas�",
            Description = "Luego de despertarnos, fuimos al club nocturno ruso que nuestro supuesto contacto nos dej� marcado, el patovica nos estaba esperando, lo cual, aparte de nuestro contacto, es posible que nos est�n esperando m�s personas.\r\n\nObjetivos:\r\n-\tEncontrarse con nuestro contacto\r\n-\tDefenderse de cualquier amenaza\r\n-\tObtener Informaci�n sobre nuestra situaci�n\r\n",
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
        public string ShortDescription { get; set; }
        public string SceneName { get; set; }
        public LevelType LevelType { get; set; }
        public List<LevelCode> Unlocks { get; set; }
        public LevelCode NextLevel { get; set; }
        public bool HasPredefinedCharacters { get; set; }
        public bool IsPreviousLevelCutscene { get; set; }
    }
}