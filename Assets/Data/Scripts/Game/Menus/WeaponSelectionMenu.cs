using Keru.Scripts.Engine.Master;
using Keru.Scripts.Game.ScriptableObjects;
using Keru.Scripts.Game.ScriptableObjects.Models;
using Keru.Scripts.Game.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Keru.Scripts.Game.Menus
{
    public class WeaponSelectionMenu : MonoBehaviour
    {
        [SerializeField] private List<GunStats> _allWeapons;
        [SerializeField] private List<WeaponThirdPersonModel> _primaryWeaponsModels;
        [SerializeField] private List<WeaponThirdPersonModel> _secondaryWeaponsModels;
        [SerializeField] private Text _primaryWeaponName;
        [SerializeField] private Text _secondaryWeaponName;
        [SerializeField] private Text _primaryWeaponData;
        [SerializeField] private Text _secondaryWeaponData;
        [SerializeField] private CharacterSelectionMenu _characterSelectionMenu;
        [SerializeField] private GameObject _pasteCharacterModule;
        [SerializeField] private InputField _pastedCharacterCode;
        [SerializeField] private Text _coopCharacterData;
        [SerializeField] private AlternativeMusicMenu _alternativeMusicMenu;
        [SerializeField] Text _levelNameText;

        private int _primaryCode;
        private int _secondaryCode;
        private List<WeaponMenuData> _primaryWeapons;
        private List<WeaponMenuData> _secondaryWeapons;
        private string _levelName;

        private void OnEnable()
        {
            _levelNameText.text = $"{_levelName} -> selecciona equipamiento".ToUpper();

            var currentSave = LevelBase.CurrentSave;

            LoadWeaponDataToSelect(currentSave);

            _primaryCode = _primaryWeapons.FindIndex(x => x.Code == currentSave.SelectedCharacter.Primary.Code);
            ChangePrimaryWeapon(0);

            _secondaryCode = _secondaryWeapons.FindIndex(x => x.Code == currentSave.SelectedCharacter.Secondary.Code);
            ChangeSecondaryWeapon(0);
        }

        public void SetLevelName(string levelName)
        {
            _levelName = levelName;
        }

        private void LoadWeaponDataToSelect(SaveGameFile currentSave)
        {
            _primaryWeapons = new();
            _secondaryWeapons = new();

            var allWeaponsSaveData = currentSave.Weapons;
            foreach (var weapon in _allWeapons)
            {
                var weaponSaveData = allWeaponsSaveData.FirstOrDefault(x => x.Code == weapon.WeaponCode && x.Unlocked == true);
                if (weaponSaveData != null)
                {
                    var weaponMenuData = new WeaponMenuData()
                    {
                        Code = weapon.WeaponCode,
                        WeaponName = weapon.name,
                        Level = weaponSaveData.Level,
                        CurrentKills = weaponSaveData.CurrentKills,
                        RequiredKills = weaponSaveData.KillsRequired,
                        WeaponData = weapon.WeaponDataPerLevel[weaponSaveData.Level - 1]
                    };

                    if (weapon.WeaponSlot == WeaponSlot.Primary)
                    {
                        weaponMenuData.WeaponModel = _primaryWeaponsModels.First(x => x.WeaponData.WeaponCode == weapon.WeaponCode);
                        _primaryWeapons.Add(weaponMenuData);
                    }
                    else
                    {
                        weaponMenuData.WeaponModel = _secondaryWeaponsModels.First(x => x.WeaponData.WeaponCode == weapon.WeaponCode);
                        _secondaryWeapons.Add(weaponMenuData);
                    }
                }
            }
        }

        public void GoBack()
        {
            _characterSelectionMenu.gameObject.SetActive(true);
            DisableAllWeapons(_primaryWeapons);
            DisableAllWeapons(_secondaryWeapons);
            gameObject.SetActive(false);
        }

        public void Continue()
        {
            DisableAllWeapons(_primaryWeapons);
            DisableAllWeapons(_secondaryWeapons);

            var savedGame = LevelBase.CurrentSave.SelectedCharacter;
            var finalPrimaryWeapon = _primaryWeapons[_primaryCode];
            var finalSecondaryWeapon = _secondaryWeapons[_secondaryCode];

            savedGame.Primary = new WeaponData()
            {
                Level = finalPrimaryWeapon.Level,
                Code = finalPrimaryWeapon.Code,
                AmmoType = finalPrimaryWeapon.WeaponData.AmmoType,
                CurrentBulletsInMag = finalPrimaryWeapon.WeaponData.MagazineSize,
                CurrentKills = finalPrimaryWeapon.CurrentKills,
                KillsAmmountPerLevel = 0,
                KillsRequired = 0,
                Unlocked = true
            };
            savedGame.Secondary = new WeaponData()
            {
                Level = finalSecondaryWeapon.Level,
                Code = finalSecondaryWeapon.Code,
                AmmoType = finalSecondaryWeapon.WeaponData.AmmoType,
                CurrentBulletsInMag = finalSecondaryWeapon.WeaponData.MagazineSize,
                CurrentKills = finalSecondaryWeapon.CurrentKills,
                KillsAmmountPerLevel = 0,
                KillsRequired = 0,
                Unlocked = true
            };

            _alternativeMusicMenu.SetLevelName(_levelNameText.text);
            _alternativeMusicMenu.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }

        public void CopyPlayerCode()
        {
            _pasteCharacterModule.SetActive(false);
            var currentSave = LevelBase.CurrentSave;
            var selectedCharacter = currentSave.SelectedCharacter.Character;
            var currentCharacterLevel = currentSave.Characters.First(x => x.Character == selectedCharacter).Level;
            var primaryWeaponCode = currentSave.SelectedCharacter.Primary.Code;
            var primaryWeaponLevel = currentSave.Weapons.First(x => x.Code == primaryWeaponCode).Level;
            var secondaryWeaponCode = currentSave.SelectedCharacter.Secondary.Code;
            var secondaryWeaponLevel = currentSave.Weapons.First(x => x.Code == secondaryWeaponCode).Level;

            var copyString = $"{(int)selectedCharacter}/{currentCharacterLevel}/{(int)primaryWeaponCode}/{primaryWeaponLevel}/{(int)secondaryWeaponCode}/{secondaryWeaponLevel}";
            GUIUtility.systemCopyBuffer = copyString;
        }

        public void PasteCoopCode()
        {
            try
            {
                var characterCodes = _pastedCharacterCode.text;
                var codes = characterCodes.Split("/");
                Character character;
                WeaponCodes primaryWeapon;
                WeaponCodes secondaryWeapon;

                if (Enum.IsDefined(typeof(Character), int.Parse(codes[0])))
                {
                    character = (Character)Enum.Parse(typeof(Character), codes[0]);
                }
                else
                {
                    throw new Exception();
                }
                var characterLevel = int.Parse(codes[1]);


                if (Enum.IsDefined(typeof(WeaponCodes), int.Parse(codes[2])))
                {
                    primaryWeapon = (WeaponCodes)Enum.Parse(typeof(WeaponCodes), codes[2]);
                }
                else
                {
                    throw new Exception();
                }
                var primaryWeaponLevel = int.Parse(codes[3]);

                if (Enum.IsDefined(typeof(WeaponCodes), int.Parse(codes[4])))
                {
                    secondaryWeapon = (WeaponCodes)Enum.Parse(typeof(WeaponCodes), codes[4]);
                }
                else
                {
                    throw new Exception();
                }
                var secondaryWeaponLevel = int.Parse(codes[5]);

                if (primaryWeaponLevel < 0 || secondaryWeaponLevel < 0 || characterLevel < 0)
                {
                    throw new Exception();
                }
                if (primaryWeaponLevel > 4 || secondaryWeaponLevel > 4 || characterLevel > 4)
                {
                    throw new Exception();
                }

                LevelBase.CurrentSave.CoopCharacterData = characterCodes;
                _coopCharacterData.text = $"{character} ({characterLevel}) - {primaryWeapon}({primaryWeaponLevel}) - {secondaryWeapon}({secondaryWeaponLevel})";
            }
            catch (Exception)
            {
                _coopCharacterData.text = "Codigo con formato incorrecto".ToUpper();
            }
        }

        public void ChangePrimaryWeapon(int direction)
        {
            _primaryCode += direction;
            if (_primaryCode == _primaryWeapons.Count)
            {
                _primaryCode = 0;
            }
            else if (_primaryCode < 0)
            {
                _primaryCode = _primaryWeapons.Count - 1;
            }

            DisableAllWeapons(_primaryWeapons);
            SelectWeapon(_primaryWeaponName, _primaryWeaponData, _primaryWeapons[_primaryCode]);
        }

        public void ChangeSecondaryWeapon(int direction)
        {
            _secondaryCode += direction;
            if (_secondaryCode == _secondaryWeapons.Count)
            {
                _secondaryCode = 0;
            }
            else if (_secondaryCode < 0)
            {
                _secondaryCode = _secondaryWeapons.Count - 1;
            }

            DisableAllWeapons(_secondaryWeapons);
            SelectWeapon(_secondaryWeaponName, _secondaryWeaponData, _secondaryWeapons[_secondaryCode]);
        }

        private void SelectWeapon(Text weaponName, Text weaponData, WeaponMenuData weaponMenuData)
        {
            weaponMenuData.WeaponModel.gameObject.SetActive(true);
            weaponName.text = $"{weaponMenuData.WeaponName} - NIVEL {weaponMenuData.Level} ({weaponMenuData.CurrentKills}/{weaponMenuData.RequiredKills})".ToUpper();
            weaponData.text = $"Daño: {weaponMenuData.WeaponData.Damage} | Munición: {weaponMenuData.WeaponData.AmmoType} | Cargador: {weaponMenuData.WeaponData.MagazineSize} | Cadencia: {weaponMenuData.WeaponData.FireRate * 60} por Minuto".ToUpper();
        }

        private void DisableAllWeapons(List<WeaponMenuData> weapons)
        {
            foreach (var weapon in weapons)
            {
                weapon.WeaponModel.gameObject.SetActive(false);
            }
        }
    }

    public class WeaponMenuData
    {
        public WeaponCodes Code { get; set; }
        public string WeaponName { get; set; }
        public int Level { get; set; }
        public int CurrentKills { get; set; }
        public int RequiredKills { get; set; }
        public WeaponLevel WeaponData { get; set; }
        public WeaponThirdPersonModel WeaponModel { get; set; }
    }
}