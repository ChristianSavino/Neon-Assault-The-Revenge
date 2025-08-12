using Keru.Scripts.Game.Entities.Humanoid;
using Keru.Scripts.Game.ScriptableObjects;
using Keru.Scripts.Game.Weapons;
using Keru.Scripts.Game.Weapons.NPCs;
using System.Linq;
using UnityEngine;

namespace Keru.Scripts.Game.Entities.NPCs
{
    public class NPCWeaponHandler : WeaponHandler
    {
        [SerializeField] private GameObject _leftHand;

        public void SetConfig(ThirdPersonAnimations personAnimations, NPCStats npcStats)
        {
            _animations = personAnimations;
            if(npcStats.PrimaryWeaponCodes.Any())
            {
                var primaryCode = npcStats.PrimaryWeaponCodes[Random.Range(0, npcStats.PrimaryWeaponCodes.Count)];
                var primaryWeaponModel = _animations.GetWeaponModel(primaryCode);
                _primaryWeapon = primaryWeaponModel.AddComponent<NPCWeapon>();
                _primaryWeapon.SetConfig(this, primaryWeaponModel, _leftHand, npcStats.PrimaryWeaponLevel, owner: gameObject);
                DeployWeapon(_primaryWeapon);
            }

            if(npcStats.SecondaryWeaponCodes.Any())
            {
                var secondaryCode = npcStats.SecondaryWeaponCodes[Random.Range(0, npcStats.SecondaryWeaponCodes.Count)];
                var secondaryWeaponModel = _animations.GetWeaponModel(secondaryCode);
                _secondaryWeapon = secondaryWeaponModel.AddComponent<NPCWeapon>();
                _secondaryWeapon.SetConfig(this, secondaryWeaponModel, _leftHand, npcStats.SecondaryWeaponLevel, owner: gameObject);
                if(_primaryWeapon == null)
                {
                    DeployWeapon(_secondaryWeapon);
                }
            }  
        }

        public void SetAccuracyMultiplier(float multiplier)
        {
            if (_primaryWeapon != null)
            {
                _primaryWeapon.SetAccuracy(multiplier);
            }
            if (_secondaryWeapon != null)
            {
                _secondaryWeapon.SetAccuracy(multiplier);
            }
        }

        public void DeployWeapon(WeaponSlot weaponSlot)
        {
            switch (weaponSlot)
            {
                case WeaponSlot.PRIMARY:
                    DeployWeapon(_primaryWeapon);
                    break;
                case WeaponSlot.SECONDARY:
                    DeployWeapon(_secondaryWeapon);
                    break;
            }
        }

        public override void DeployWeapon(Weapon weapon, bool forcedDeploy = false, bool isMelee = false)
        {
            ToggleWeapons(false);

            if (weapon != null)
            {
                _currentWeapon = weapon;
            }

            _currentWeapon.gameObject.SetActive(true);
            _currentWeapon.Deploy();
        }

        public void Shoot(Vector3 direction)
        {
            _currentWeapon.Shoot(direction);
        }

        public bool HasBulletsInMag()
        {
            return _currentWeapon != null && _currentWeapon.GetCurrentBulletsInMag() > 0;
        }

        public bool HasBulletsInTotal()
        {
            return _currentWeapon != null && _currentWeapon.GetCurrentTotalBullets() > 0;
        }

        public bool CanShoot()
        {
            return _currentWeapon != null && _currentWeapon.AbleToShoot();
        }

        public void Reload()
        {
            if (_currentWeapon != null)
            {
                _currentWeapon.Reload();
            }
        }

        public void StopShooting()
        {
            if (_currentWeapon != null)
            {
                _currentWeapon.EndsShoot();
            }
        }

        public void RefillSecondaryAmmo()
        {
            if (_secondaryWeapon != null)
            {
                _secondaryWeapon.RefillMaxAmmo(1);
            }
        }

        public WeaponSlot GetCurrentWeaponSlot()
        {
            if (_currentWeapon == _primaryWeapon)
            {
                return WeaponSlot.PRIMARY;
            }
            else if (_currentWeapon == _secondaryWeapon)
            {
                return WeaponSlot.SECONDARY;
            }
            return WeaponSlot.MELEE;
        }
    }
}