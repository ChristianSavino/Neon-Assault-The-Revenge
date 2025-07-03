using Keru.Scripts.Game;

namespace Keru.Scripts.Helpers
{
    public class AnimationNamesHelper
    {
        public const string JumpAnimation = "Jump";
        public const string DashAnimation = "Dash";
        public const string WeaponShootAnimation = "_Shoot";
        public const string WeaponReloadAnimation = "_Reload";
        public const string WeaponEmptyReloadAnimation = "_Reload_Empty";
        public const string WeaponReloadOpenAnimation = "_Reload_Open";
        public const string WeaponReloadCloseAnimation = "_Reload_Close";
        public const string WeaponReloadInsertAnimation = "_Reload_Insert";
        public const string WeaponIdleAnimation = "_Idle";
        public const string WeaponAimAnimation = "_Aim";
    }

    public class ParameterNamesHelper
    {
        public const string OnLandParameter = "onLand";
        public const string YParameter = "Y";
        public const string XParameter = "X";
        public const string IsCrouchingParameter = "isCrouching";
    }

    public static class WeaponAnimationNamesHelper
    {
        public static string ReturnAnimationWeaponName(WeaponCodes weaponCode)
        {
            switch (weaponCode)
            {
                case WeaponCodes.GLOCK17:
                case WeaponCodes.BERETTAM9:
                case WeaponCodes.M1911:
                    return "Pistol";
                case WeaponCodes.TEC9:
                    return "Tec9";
                case WeaponCodes.SHOTGUN:
                    return "Shotgun";
                case WeaponCodes.UZI:
                    return "Uzi";
                case WeaponCodes.VECTOR:
                    return "Vector";
                case WeaponCodes.MP5:
                    return "MP5";
                case WeaponCodes.P90:
                    return "P90";
                case WeaponCodes.M4A1:
                    return "M4";
                case WeaponCodes.AK47:
                    return "AK47";
                case WeaponCodes.SCARH:
                    return "Scar";
                case WeaponCodes.GL40:
                    return "GL";
                case WeaponCodes.RPG:
                    return "RPG";
                case WeaponCodes.KATANA:
                    return "Katana";
                default:
                    return "Idle";
            }
        }
    }
}