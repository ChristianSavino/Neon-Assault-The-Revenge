using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ControlMapping
{
    public Dictionary<string, KeyCode> Keys { get; set; }
    public float Sensibility { get; set; }
    public bool AutoReload { get; set; }

    public ControlMapping()
    {
        Keys = new Dictionary<string, KeyCode>()
        {
            {"Up", KeyCode.W},
            {"Left", KeyCode.A},
            {"Right", KeyCode.D},
            {"Back", KeyCode.S},
            {"Dash", KeyCode.LeftShift},
            {"Jump", KeyCode.Space},
            {"Duck", KeyCode.LeftControl},
            {"Shoot", KeyCode.Mouse0},
            {"Reload", KeyCode.R},
            {"Special", KeyCode.Q},
            {"Second", KeyCode.Mouse1},
            {"Pause", KeyCode.Escape},
            {"Use", KeyCode.E},
            {"Knife", KeyCode.F},
            {"Grenade", KeyCode.G},
        };
        
        Sensibility = 5;
        AutoReload = true;
    }
}
