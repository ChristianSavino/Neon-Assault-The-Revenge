using Keru.Scripts.Game;
using System.Collections.Generic;

[System.Serializable]
public class AbilityData
{
    public AbilityCodes Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public AbilitySlot AbilitySlot { get; set; }
    public AbilityType AbilityType { get; set; }
    public List<AbilityDataPerLevel> DataPerLevel { get; set; }
}

public class AbilityDataPerLevel
{
    public float Power { get; set; }
    public float Range { get; set; }
    public float Duration { get; set; }
    public float CoolDown { get; set; }
}
