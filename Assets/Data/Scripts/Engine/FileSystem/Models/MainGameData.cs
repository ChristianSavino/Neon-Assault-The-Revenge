using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MainGameData
{
    public string Version { get; set; }
    public int SaveGameLocation { get; set; }
    public bool AlternateMusic { get; set; }
    public List<Achievements> Achievements { get; set; }
    public Options Options { get; set; }
    public MainGameData()
    {
        Version = Application.version;
        SaveGameLocation = -1;
        Achievements = new List<Achievements>();
        Options = new Options();
        Options.AudioOptions = new AudioOptions();
        Options.GraphicsOptions = new GraphicsOptions();
        Options.PlayerControls = new ControlMapping();
    }
}

public class Achievements
{
    public int Id { get; set; }

    public string Name { get; set; }

    public bool Completed { get; set; }
}

public class Options
{
    public ControlMapping PlayerControls { get; set; }

    public GraphicsOptions GraphicsOptions { get; set; }

    public AudioOptions AudioOptions { get; set; }

}
