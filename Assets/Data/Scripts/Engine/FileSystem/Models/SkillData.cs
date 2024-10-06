using Keru.Scripts.Game;
using System;

[Serializable]
public class SkillData
{
    public AbilityCodes Code {  get; set; }
    public int Level { get; set; }
    public int CurrentUses { get; set; }
}