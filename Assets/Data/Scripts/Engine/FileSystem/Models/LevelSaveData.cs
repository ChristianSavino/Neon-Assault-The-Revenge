using Keru.Scripts.Engine;
using System.Collections.Generic;
using System;

[Serializable]
public class LevelSaveData
{
    public LevelCode Code { get; set; }
    public TimeSpan CompletedTime { get; set; }
    public bool Completed { get; set; }
}
