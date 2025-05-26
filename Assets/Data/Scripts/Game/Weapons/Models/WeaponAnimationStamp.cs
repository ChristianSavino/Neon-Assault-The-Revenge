using System;

namespace Keru.Scripts.Game.Weapons.Models
{
    [Serializable]
    public class WeaponAnimationStamp
    {
        public WeaponAnimationStampType AnimType;
        public float Time;
    }

    public enum WeaponAnimationStampType
    {
        NONE = 0,
        WEAPON = 1,
        LEFT_HAND = 2
    }
}