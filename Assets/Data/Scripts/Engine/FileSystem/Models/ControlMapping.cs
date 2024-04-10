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
        Keys = new Dictionary<string, KeyCode>();
        Keys.Add("Up", KeyCode.W);
        Keys.Add("Left", KeyCode.A);
        Keys.Add("Right", KeyCode.D);
        Keys.Add("Back", KeyCode.S);
        Keys.Add("Dash", KeyCode.LeftShift);
        Keys.Add("Jump", KeyCode.Space);
        Keys.Add("Duck", KeyCode.LeftControl);
        Keys.Add("Shoot", KeyCode.Mouse0);
        Keys.Add("Reload", KeyCode.R);
        Keys.Add("Flashlight", KeyCode.H);
        Keys.Add("Special", KeyCode.Q);
        Keys.Add("Second", KeyCode.Mouse1);
        Keys.Add("Pause", KeyCode.Escape);
        Keys.Add("Use", KeyCode.E);
        Keys.Add("Knife", KeyCode.F);
        Keys.Add("Grenade", KeyCode.G);
        Sensibility = 5;
        AutoReload = true;
    }
}
