using Keru.Scripts.Engine;
using Keru.Scripts.Game;
using System;
using System.Collections.Generic;

[System.Serializable]
public class SaveGameFile
{
    public int SavePosition { get; set; }
    public int Checkpoint { get; set; }
    public LevelCode CurrentLevelCode { get; set; }
    public CurrentCharacterData SelectedCharacter { get; set; }
    public string CoopCharacterData { get; set; }
    public DateTime LastSaveDate { get; set; }
    public List<LevelSaveData> AllLevelData { get; set; }
    public List<CharacterData> Characters { get; set; }
    public List<WeaponData> Weapons { get; set; }
    public bool AlternateMusic { get; set; }

    public SaveGameFile(int savePosition)
    {
        SavePosition = savePosition;
        Checkpoint = 0;
        CurrentLevelCode = LevelCode.Level0;
        CreateLevelData();
        CreateCharacterData();
        CreateWeaponData();
        SelectedCharacter = new CurrentCharacterData()
        {
            Character = Character.KERU,
            CurrentHealth = 100,
            MaxHealth = 100,
            Primary = Weapons[4],
            Secondary = Weapons[2]
           };
        LastSaveDate = DateTime.Now;       
    }

    private void CreateCharacterData()
    {
        Characters = new List<CharacterData>()
        {
            new CharacterData()
            {
                Character = Character.KERU,
                Level = 1,
                BaseHealth = 100,
                HealthPerLevel = 25,
                Secondary = new AbilityData()
                {
                    Code = 0,
                    AbilityType = AbilityType.CROWDCONTROL,
                    PowerPerLevel = "0/0/0/0",
                    RangePerLevel = "0/0/0/0",
                    DurationPerLevel = "5/8/9/10",
                    CoolDownPerLevel = "60/40/30/20"
                },
                Primary = new AbilityData()
                {
                    Code = 1,
                    AbilityType = AbilityType.DAMAGE,
                    PowerPerLevel = "1000/1500/2000/2500",
                    RangePerLevel = "20/30/40/50",
                    DurationPerLevel = "0/0/0/0",
                    CoolDownPerLevel = "180/150/120/90"
                },
                Exp = 0,
                ExpAmmountPerLevel = 2,
                ExpToLevelUp = 2
            },
            new CharacterData()
            {
                Character = Character.BEDU,
                Level = 1,
                BaseHealth = 100,
                HealthPerLevel = 20,
                Secondary = new AbilityData()
                {
                    Code = 0,
                    AbilityType = AbilityType.SUPPORT,
                    PowerPerLevel = "15/20/25/30",
                    RangePerLevel = "15/20/30/40",
                    DurationPerLevel = "7/8/9/10",
                    CoolDownPerLevel = "90/80/70/60"
                },
                Primary = new AbilityData()
                {
                    Code = 1,
                    AbilityType = AbilityType.CROWDCONTROL,
                    PowerPerLevel = "0/0/0/0",
                    RangePerLevel = "20/30/40/50",
                    DurationPerLevel = "9/10/11/12",
                    CoolDownPerLevel = "180/150/120/90"
                },
                Exp = 0,
                ExpAmmountPerLevel = 2,
                ExpToLevelUp = 2
            },
            new CharacterData()
            {
                Character = Character.ANGEL,
                Level = 1,
                BaseHealth = 100,
                HealthPerLevel = 20,
                Secondary = new AbilityData()
                {
                    Code = 0,
                    AbilityType = AbilityType.CROWDCONTROL,
                    PowerPerLevel = "10/15/20/25",
                    RangePerLevel = "8/10/20/30",
                    DurationPerLevel = "3/4/5/6",
                    CoolDownPerLevel = "80/70/60/50"
                },
                Primary = new AbilityData()
                {
                    Code = 1,
                    AbilityType = AbilityType.DAMAGE,
                    PowerPerLevel = "250/500/750/1000",
                    RangePerLevel = "40/50/60/70",
                    DurationPerLevel = "3/4/5/6",
                    CoolDownPerLevel = "180/150/120/90"
                },
                Exp = 0,
                ExpAmmountPerLevel = 2,
                ExpToLevelUp = 2
            },
            new CharacterData()
            {
                Character = Character.MAURO,
                Level = 1,
                BaseHealth = 100,
                HealthPerLevel = 20,
                Secondary = new AbilityData()
                {
                    Code = 0,
                    AbilityType = AbilityType.SUPPORT,
                    PowerPerLevel = "5/10/15/20",
                    RangePerLevel = "8/10/20/30",
                    DurationPerLevel = "3/4/5/6",
                    CoolDownPerLevel = "120/100/80/60"
                },
                Primary = new AbilityData()
                {
                    Code = 1,
                    AbilityType = AbilityType.DAMAGE,
                    PowerPerLevel = "500/600/700/800",
                    RangePerLevel = "3/4/5/6",
                    DurationPerLevel = "0/0/0/0",
                    CoolDownPerLevel = "250/200/150/100"
                },
                Exp = 0,
                ExpAmmountPerLevel = 2,
                ExpToLevelUp = 2
            },
        };
    }

    private void CreateWeaponData()
    {
        Weapons = new List<WeaponData>()
        {
            new WeaponData()
            {
                Code = WeaponCodes.GLOCK17,
                AmmoType = AmmoType.MM9,
                CurrentBulletsInMag = 17,
                Level = 1,
                CurrentKills = 0,
                KillsRequired = 60,
                KillsAmmountPerLevel = 20,
                Unlocked = true
            },
            new WeaponData()
            {
                Code = WeaponCodes.BERETTAM9,
                AmmoType = AmmoType.MM9,
                CurrentBulletsInMag = 15,
                Level = 1,
                CurrentKills = 0,
                KillsRequired = 60,
                KillsAmmountPerLevel = 20,
                Unlocked = true
            },
            new WeaponData()
            {
                Code = WeaponCodes.M1911,
                AmmoType = AmmoType.CAL45,
                CurrentBulletsInMag = 7,
                Level = 1,
                CurrentKills = 0,
                KillsRequired = 60,
                KillsAmmountPerLevel = 20,
                Unlocked = true
            },
            new WeaponData()
            {
                Code = WeaponCodes.TEC9,
                AmmoType = AmmoType.MM9,
                CurrentBulletsInMag = 20,
                Level = 1,
                CurrentKills = 0,
                KillsRequired = 80,
                KillsAmmountPerLevel = 20,
                Unlocked = true
            },
            new WeaponData()
            {
                Code = WeaponCodes.SHOTGUN,
                AmmoType = AmmoType.G12,
                CurrentBulletsInMag = 8,
                Level = 1,
                CurrentKills = 0,
                KillsRequired = 100,
                KillsAmmountPerLevel = 50,
                Unlocked = true
            },
            new WeaponData()
            {
                Code = WeaponCodes.UZI,
                AmmoType = AmmoType.MM9,
                CurrentBulletsInMag = 25,
                Level = 1,
                CurrentKills = 0,
                KillsRequired = 100,
                KillsAmmountPerLevel = 30,
                Unlocked = true
            },
            new WeaponData()
            {
                Code = WeaponCodes.VECTOR,
                AmmoType = AmmoType.MM9,
                CurrentBulletsInMag = 33,
                Level = 1,
                CurrentKills = 0,
                KillsRequired = 100,
                KillsAmmountPerLevel = 50,
                Unlocked = false
            },
            new WeaponData()
            {
                Code = WeaponCodes.MP5,
                AmmoType = AmmoType.MM9,
                CurrentBulletsInMag = 30,
                Level = 1,
                CurrentKills = 0,
                KillsRequired = 100,
                KillsAmmountPerLevel = 50,
                Unlocked = false
            },
            new WeaponData()
            {
                Code = WeaponCodes.P90,
                AmmoType = AmmoType.CAL45,
                CurrentBulletsInMag = 50,
                Level = 1,
                CurrentKills = 0,
                KillsRequired = 100,
                KillsAmmountPerLevel = 50,
                Unlocked = false
            },
            new WeaponData()
            {
                Code = WeaponCodes.M4A1,
                AmmoType = AmmoType.CAL556,
                CurrentBulletsInMag = 30,
                Level = 1,
                CurrentKills = 0,
                KillsRequired = 150,
                KillsAmmountPerLevel = 40,
                Unlocked = false
            },
            new WeaponData()
            {
                Code = WeaponCodes.AK47,
                AmmoType = AmmoType.CAL762,
                CurrentBulletsInMag = 30,
                Level = 1,
                CurrentKills = 0,
                KillsRequired = 150,
                KillsAmmountPerLevel = 50,
                Unlocked = false
            },
            new WeaponData()
            {
                Code = WeaponCodes.SCARH,
                AmmoType = AmmoType.CAL762,
                CurrentBulletsInMag = 20,
                Level = 1,
                CurrentKills = 0,
                KillsRequired = 200,
                KillsAmmountPerLevel = 50,
                Unlocked = false
            },
            new WeaponData()
            {
                Code = WeaponCodes.GL40,
                AmmoType = AmmoType.GRENADE,
                CurrentBulletsInMag = 1,
                Level = 1,
                CurrentKills = 0,
                KillsRequired = 50,
                KillsAmmountPerLevel = 10,
                Unlocked = false
            },
            new WeaponData()
            {
                Code = WeaponCodes.RPG,
                AmmoType = AmmoType.ROCKET,
                CurrentBulletsInMag = 1,
                Level = 1,
                CurrentKills = 0,
                KillsRequired = 30,
                KillsAmmountPerLevel = 10,
                Unlocked = false
            },
        };
    }

    private void CreateLevelData()
    {
        AllLevelData = new List<LevelSaveData>()
        {
            new LevelSaveData()
            {
                Code = LevelCode.Level0,
                Completed = false
            }
        };
    }
}
