using Keru.Scripts.Game;

[System.Serializable]
public class AbilityData
{
    public int Code { get; set; }
    public AbilitySlot AbilitySlot { get; set; }
    public AbilityType AbilityType { get; set; }
    public string PowerPerLevel { get; set; }
    public string RangePerLevel { get; set; }
    public string DurationPerLevel { get; set; }
    public string CoolDownPerLevel { get; set; }
}
