using Keru.Scripts.Engine.Master;
using Keru.Scripts.Game.ScriptableObjects;
using Keru.Scripts.Game.Weapons;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Keru.Scripts.Game.Menus.GameMainMenu.WeaponSelection
{
    public class WeaponSelectionData : MonoBehaviour
    {
        private List<GunStats> _weapons;
        private int _currentIndex;
        private bool _isPrimary;
        private GameObject _previousWeaponModel;

        [Header("UI")]
        [SerializeField] private Text _weaponName;
        [SerializeField] private Text _damage, _magazine, _level, _kills;

        [Header("Weapon Models")]
        [SerializeField] private List<WeaponThirdPersonModel> _weaponModels;

        public void SetWeaponList(List<GunStats> weapons, bool isPrimary)
        {
            if (_weapons == null)
            {
                _weapons = weapons;
                _isPrimary = isPrimary;
                SetWeaponData(0);
            }
        }

        public void SetWeaponData(int direction)
        {
            if (direction == 0)
            {
                var code = WeaponCodes.GLOCK17;
                var saveGameWeapons = LevelBase.CurrentSave.CurrentCharacterData;
                if (_isPrimary)
                {
                    code = saveGameWeapons.Primary.Code;
                }
                else
                {
                    code = saveGameWeapons.Secondary.Code;
                }

                _currentIndex = _weapons.IndexOf(_weapons.Where(x => x.WeaponCode == code).First());
            }
            else
            {
                _currentIndex += direction;
                if (_currentIndex >= _weapons.Count)
                {
                    _currentIndex = 0;
                }
                if (_currentIndex < 0)
                {
                    _currentIndex = _weapons.Count - 1;
                }

            }

            if (_previousWeaponModel != null)
            {
                _previousWeaponModel.SetActive(false);
            }

            ShowWeaponData(_weapons[_currentIndex]);
        }

        private void ShowWeaponData(GunStats weapon)
        {
            var weaponData = LevelBase.CurrentSave.Weapons.Where(x => x.Code == weapon.WeaponCode).First();
            var weaponModel = _weaponModels.Where(x => x.WeaponData.WeaponCode == weapon.WeaponCode).First();

            weaponModel.gameObject.SetActive(true);
            _previousWeaponModel = weaponModel.gameObject;

            _weaponName.text = weapon.name.ToUpper();
            var weaponSubData = weapon.WeaponDataPerLevel[weaponData.Level-1];

            _damage.text = $"Daño: {weaponSubData.Damage}".ToUpper();
            _magazine.text = $"Cargador: {weaponSubData.MagazineSize}".ToUpper();
            _level.text = $"Nivel: {weaponData.Level}".ToUpper();
            _kills.text = $"Bajas: {weaponData.CurrentKills}/{weaponData.KillsRequired + (weaponData.KillsAmmountPerLevel * (weaponData.Level - 1))}".ToUpper();
        }

        public WeaponData GetCurrentWeaponData()
        {
            return LevelBase.CurrentSave.Weapons.Where(x => x.Code == _weapons[_currentIndex].WeaponCode).First();
        }
    }
}