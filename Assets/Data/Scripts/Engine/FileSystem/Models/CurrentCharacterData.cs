using Keru.Scripts.Game;

[System.Serializable]
public class CurrentCharacterData
{
    public Character Character { get; set; }
    public int CurrentHealth { get; set; }
    public int MaxHealth { get; set; }
    public WeaponData Secondary { get; set; }
    public WeaponData Primary { get; set; }
}
