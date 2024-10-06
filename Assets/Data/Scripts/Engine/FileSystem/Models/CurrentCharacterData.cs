using Keru.Scripts.Game;

[System.Serializable]
public class CurrentCharacterData
{
    public int CurrentHealth { get; set; }
    public int MaxHealth { get; set; }
    public WeaponData Secondary { get; set; }
    public WeaponData Primary { get; set; }
    public int UltimateSkill { get; set; }
    public int SecondarySkill { get; set; }
}
