using Keru.Scripts.Game;

[System.Serializable]
public class WeaponData
{
    public WeaponCodes Code { get; set; }
    public AmmoType AmmoType { get; set; }
    public int CurrentBulletsInMag { get; set; }
    public int Level { get; set; }
    public int CurrentKills { get; set; }
    public int KillsRequired { get; set; }
    public int KillsAmmountPerLevel { get; set; }
    public bool Unlocked { get; set; }
}
