namespace Keru.Scripts.Game
{
    public enum AbilitySlot
    {
        ULTIMATE = 0,
        SECONDARY = 1
    }

    public enum AbilityAction
    {
        IDLE = 0,
        CAST = 1,
        TOGGLE = 2,
        COOLDOWN = 3
    }

    public enum AbilityType
    {
        DAMAGE = 0,
        CROWDCONTROL = 1,
        SUPPORT = 2
    }

    public enum AbilityCodes
    {
        BULLETTIME = 0,
        JUDGEMENTCUT = 1
    }

    public enum WeaponCodes
    {
        GLOCK17 = 0,
        BERETTAM9 = 1,
        M1911 = 2,
        TEC9 = 3,
        SHOTGUN = 4,
        UZI = 5,
        VECTOR = 6,
        MP5 = 7,
        P90 = 8,
        M4A1 = 9,
        AK47 = 10,
        SCARH = 11,
        GL40 = 12,
        RPG = 13,
        KATANA = 14
    }

    public enum AmmoType
    {
        MM9 = 0,
        G12 = 1,
        CAL45 = 2,
        CAL556 = 3,
        CAL762 = 4,
        CAL50 = 5,
        ROCKET = 6,
        GRENADE = 7,
        SPECIAL = 8,
        MELEE = 9
    }

    public enum WeaponType
    {
        SEMI = 0,
        SHOTGUN = 1,
        AUTO = 2
    }

    public enum WeaponSlot
    {
        PRIMARY = 0,
        SECONDARY = 1,
        MELEE = 2
    }

    public enum DamageType
    {
        TRUE_DAMAGE = 0,
        MELEE = 1,
        BULLET = 2,
        EXPLOSION = 3,
        FIRE = 4,
        LIGHTNING = 5,
        POISON = 6
    }

    public enum AnimationLayers
    {
        BASE = 0,
        WEAPON = 1,
        SPECIAL = 2
    }

    public enum WeaponActions
    {
        DEPLOY = 0,
        SHOOT = 1,
        RELOAD = 2,
        RELOAD_EMPTY = 3,
        RELOAD_OPEN = 4,
        RELOAD_CLOSE = 5,
        RELOAD_INSERT = 6
    }
}
