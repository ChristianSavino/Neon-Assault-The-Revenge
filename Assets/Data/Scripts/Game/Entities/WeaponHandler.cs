using Keru.Scripts.Game.Entities.Humanoid;
using Keru.Scripts.Game.Weapons;
using UnityEngine;

namespace Keru.Scripts.Game.Entities
{
    public class WeaponHandler : MonoBehaviour
    {
        protected Weapon _primaryWeapon;
        protected Weapon _secondaryWeapon;
        protected Weapon _meleeWeapon;

        protected Weapon _currentWeapon;
        protected ThirdPersonAnimations _animations;

        public virtual void DeployWeapon(Weapon weapon, bool forcedDeploy = false, bool isMelee = false)
        {

        }

        protected virtual void ToggleWeapons(bool toggle)
        {
            foreach (var weapon in new[] { _primaryWeapon, _secondaryWeapon, _meleeWeapon })
            {
                weapon?.gameObject.SetActive(toggle);
            }
        }

        public virtual void PlayAnimation(WeaponActions weaponAction, WeaponCodes weaponCodes)
        {
            if (_currentWeapon == null)
            {
                return;
            }

            _animations.PlayWeaponAnimation(weaponAction, weaponCodes);
        }

        public virtual void PlayMeleeAnimation(WeaponActions weaponAction, WeaponCodes weaponCodes)
        {
            if (_currentWeapon == null)
            {
                return;
            }
            _animations.PlayMeleeWeaponAnimation(weaponAction, weaponCodes);
        }

        public virtual void Die()
        {
            _currentWeapon?.Die();
        }

        public virtual void SetWeaponData(string name, AmmoType munitionType, int bulletsInMag, int maxBulletsInMag, int currentTotalBullets)
        {

        }

        public virtual void UpdateWeaponData(int bulletsInMag, int currentTotalBullets)
        {

        }

        public virtual void HolsterWeapons()
        {

        }
    }
}

