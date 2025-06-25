using Keru.Scripts.Engine;
using Keru.Scripts.Game;
using System;
using System.Collections.Generic;

[Serializable]
public class SaveGameFile
{
    public int SavePosition { get; set; }
    public Difficulty Difficulty { get; set; }
    public int Checkpoint { get; set; }
    public LevelCode CurrentLevelCode { get; set; }
    public CurrentCharacterData CurrentCharacterData { get; set; }
    public string CoopCharacterData { get; set; }
    public DateTime LastSaveDate { get; set; }
    public List<LevelSaveData> AllLevelData { get; set; }
    public List<SkillData> UnlockedSkills { get; set; }
    public List<WeaponData> Weapons { get; set; }
    public bool AlternateMusic { get; set; }

    public SaveGameFile(int savePosition)
    {
        SavePosition = savePosition;
        Checkpoint = 0;
        Difficulty = Difficulty.Normal;
        CurrentLevelCode = LevelCode.Level0;
        CreateLevelData();
        CreateSkillData();
        CreateWeaponData();
        CurrentCharacterData = new CurrentCharacterData()
        {
            CurrentHealth = 100,
            MaxHealth = 100,
            Primary = Weapons[4],
            Secondary = Weapons[2],
            UltimateSkill = AbilityCodes.JUDGEMENTCUT,
            SecondarySkill = AbilityCodes.BULLETTIME
           };
        LastSaveDate = DateTime.Now;       
    }

    private void CreateSkillData()
    {
        UnlockedSkills = new List<SkillData>()
        {
            new SkillData()
            {
                Code = AbilityCodes.BULLETTIME,
                CurrentUses = 0,
                Level = 1, 
                AbilitySlot = AbilitySlot.SECONDARY
            },
            new SkillData()
            {
                Code = AbilityCodes.JUDGEMENTCUT,
                CurrentUses = 0,
                Level = 1,
                AbilitySlot = AbilitySlot.ULTIMATE
            }
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
