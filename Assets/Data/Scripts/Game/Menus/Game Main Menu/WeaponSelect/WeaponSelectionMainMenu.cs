using Keru.Scripts.Engine.Master;
using Keru.Scripts.Game.ScriptableObjects;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Keru.Scripts.Game.Menus.GameMainMenu.WeaponSelection
{
    public class WeaponSelectionMainMenu : MonoBehaviour
    {
        [SerializeField] private Light _light;
        [SerializeField] private WeaponSelectionData _primaryWeapon;
        [SerializeField] private WeaponSelectionData _secondaryWeapon;
        [SerializeField] private List<GunStats> _allWeapons;

        private void OnEnable()
        {
            _light.enabled = true;
            LoadWeapons();
        }

        public void LoadWeapons()
        {
            var unlockedWeapons = LevelBase.CurrentSave.Weapons.Where(x => x.Unlocked);
            var allPrimaryWeapons = _allWeapons.Where(x => x.WeaponSlot == WeaponSlot.PRIMARY);
            var primaryWeapons = allPrimaryWeapons.Where(x => unlockedWeapons.Any(g => g.Code == x.WeaponCode));
            _primaryWeapon.SetWeaponList(primaryWeapons.ToList(), true);

            var allSecondaryWeapons = _allWeapons.Where(x => x.WeaponSlot == WeaponSlot.SECONDARY);
            var secondaryWeapons = allSecondaryWeapons.Where(x => unlockedWeapons.Any(g => g.Code == x.WeaponCode));
            _secondaryWeapon.SetWeaponList(secondaryWeapons.ToList(), false);
        }

        private void OnDisable()
        {
            _light.enabled = false;
            var saveGame = LevelBase.CurrentSave.CurrentCharacterData;
            saveGame.Primary = _primaryWeapon.GetCurrentWeaponData();
            saveGame.Secondary = _secondaryWeapon.GetCurrentWeaponData();
        }
    }
}