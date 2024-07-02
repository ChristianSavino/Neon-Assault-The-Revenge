using Keru.Scripts.Game;

[System.Serializable]
public class CharacterData
{
    public Character Character { get; set; }
    public int Level { get; set; }
    public int BaseHealth { get; set; }
    public int HealthPerLevel { get; set; }
    public AbilityData Secondary {get; set;}
    public AbilityData Primary { get; set; }
    public int Exp { get; set; }
    public int ExpToLevelUp { get; set; }
    public int ExpAmmountPerLevel { get; set; }
}