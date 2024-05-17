using Keru.Scripts.Engine;

public class LevelData
{
    public LevelCode Code { get; set; }
    public string LevelName { get; set; }
    public string SceneName { get; set; }
    public LevelType LevelType { get; set; }
    public bool Unlocked { get; set; }
    public LevelCode Unlocks { get; set; }
    public LevelCode NextLevel { get; set; }
    public bool Completed { get; set; }
}
