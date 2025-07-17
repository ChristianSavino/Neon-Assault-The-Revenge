using Keru.Scripts.Game.Entities.Player.UI;
using Keru.Scripts.Game.Weapons;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Keru.Scripts.Game.Entities.Player
{
    public class PlayerWeaponHandler : MonoBehaviour
    {
        private PlayerThirdPersonAnimations _animations;
        private WeaponUIHandler _uiHandler;
        private Weapon _primaryWeapon;
        private Weapon _secondaryWeapon;
        private Weapon _katana;
        private Weapon _currentWeapon;
        private Dictionary<string, KeyCode> _keys;
        private bool _canDeploy;
        private Vector3 _direction;
        private Camera _camera;

        [SerializeField] private GameObject _leftHand;
        [SerializeField] private WeaponCodes _meleeWeapon;

        [Header("Debug")]
        [SerializeField] private bool _debug;
        [SerializeField] private WeaponCodes _primaryForcedCode;
        [SerializeField] private int _primaryForcedLevel = 1;
        [SerializeField] private WeaponCodes _secondaryForcedCode;
        [SerializeField] private int _secondaryForcedLevel = 1;

        //Effects
        public bool CanInteract { get; set; } = true;

        private void Update()
        {
            if (CanInteract)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    DeployWeapon(_primaryWeapon);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    DeployWeapon(_secondaryWeapon);
                }
                else if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    DeployWeapon(_katana, isMelee: true);
                }

                if (_currentWeapon != null)
                {
                    CalculateDirection();
                    WeaponControls();
                }
            }
        }

        private void WeaponControls()
        {
            if (Input.GetKeyDown(_keys["Shoot"]))
            {
                _currentWeapon.StartsShoot();
            }
            if (Input.GetKeyUp(_keys["Shoot"]))
            {
                _currentWeapon.EndsShoot();
            }
            if (Input.GetKey(_keys["Shoot"]))
            {
                _currentWeapon.Shoot(_direction);
            }

            if (Input.GetKeyDown(_keys["Reload"]))
            {
                _currentWeapon.Reload();
            }
        }

        private void CalculateDirection()
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            var plane = new Plane(Vector3.up, new Vector3(0, 1, 0));
            float enter;

            var targetPoint = Vector3.zero;

            if (plane.Raycast(ray, out enter))
            {
                targetPoint = ray.GetPoint(enter);
            }

            _direction = (targetPoint - transform.position);
        }

        public void SetConfig(PlayerThirdPersonAnimations pta, SaveGameFile saveGame, Dictionary<string, KeyCode> keys, WeaponUIHandler weaponUiHandler)
        {
            _animations = pta;
            _keys = keys;

            var primaryWeaponData = !_debug ? saveGame.CurrentCharacterData.Primary : saveGame.Weapons.First(x => x.Code == _primaryForcedCode);
            var secondaryWeaponData = !_debug ? saveGame.CurrentCharacterData.Secondary : saveGame.Weapons.First(x => x.Code == _secondaryForcedCode);

            var primaryWeaponModel = _animations.GetWeaponModel(primaryWeaponData.Code);
            var secondaryWeaponModel = _animations.GetWeaponModel(secondaryWeaponData.Code);

            _primaryWeapon = primaryWeaponModel.AddComponent<Weapon>();
            _primaryWeapon.SetConfig(this, primaryWeaponModel, _leftHand, !_debug ? primaryWeaponData.Level : _primaryForcedLevel, primaryWeaponData.CurrentBulletsInMag, primaryWeaponData.CurrentTotalBullets);

            _secondaryWeapon = secondaryWeaponModel.AddComponent<Weapon>();
            _secondaryWeapon.SetConfig(this, secondaryWeaponModel, _leftHand, !_debug ? secondaryWeaponData.Level : _secondaryForcedLevel, secondaryWeaponData.CurrentBulletsInMag, secondaryWeaponData.CurrentTotalBullets);

            _camera = Camera.main;
            _uiHandler = weaponUiHandler;

            _katana = _animations.GetWeaponModel(_meleeWeapon).AddComponent<Melee>();
            _katana.SetConfig(this, _animations.GetWeaponModel(_meleeWeapon), null);
        }

        public void DeployWeapon(Weapon weapon, bool forcedDeploy = false, bool isMelee = false)
        {
            if (!_canDeploy && !forcedDeploy)
            {
                return;
            }

            _canDeploy = true;

            if (_currentWeapon != null)
            {
                if (!_currentWeapon.CanChangeWeapon())
                {
                    return;
                }
            }

            if (weapon == null)
            {
                _currentWeapon = _primaryWeapon;
            }
            else if (_currentWeapon == weapon)
            {
                return;
            }
            else
            {
                _currentWeapon = weapon;
            }

            TogglePlayerKatana(!isMelee);
            ToggleWeapons(false);
            _currentWeapon.gameObject.SetActive(true);
            _currentWeapon.Deploy();
        }

        private void ToggleWeapons(bool toggle)
        {
            _primaryWeapon.gameObject.SetActive(toggle);
            _secondaryWeapon.gameObject.SetActive(toggle);
            _katana.gameObject.SetActive(toggle);
        }

        public void PlayAnimation(WeaponActions weaponAction, WeaponCodes weaponCodes)
        {
            if (_currentWeapon == null)
            {
                return;
            }

            _animations.PlayWeaponAnimation(weaponAction, weaponCodes);
        }

        public void PlayMeleeAnimation(WeaponActions weaponAction, WeaponCodes weaponCodes)
        {
            if (_currentWeapon == null)
            {
                return;
            }
            _animations.PlayMeleeWeaponAnimation(weaponAction, weaponCodes);
        }

        public void Die()
        {
            _canDeploy = false;
            _currentWeapon?.Die();
            enabled = false;
        }

        public void SetWeaponData(string name, AmmoType munitionType, int bulletsInMag, int maxBulletsInMag, int currentTotalBullets)
        {
            _uiHandler.SetWeapon(name, munitionType, bulletsInMag, maxBulletsInMag, currentTotalBullets);
        }

        public void UpdateWeaponData(int bulletsInMag, int currentTotalBullets)
        {
            _uiHandler.UpdateBullets(bulletsInMag, currentTotalBullets);
        }

        public bool RefillAmmo(float magAmmount, bool applyForBoth, WeaponSlot weapon)
        {
            var result = false;

            if (applyForBoth)
            {
                var primaryResult = _primaryWeapon.RefillMaxAmmo(magAmmount);
                var secondaryResult = _secondaryWeapon.RefillMaxAmmo(magAmmount);
                result = primaryResult || secondaryResult;
            }
            else
            {
                if (weapon == WeaponSlot.PRIMARY)
                {
                    result = _primaryWeapon.RefillMaxAmmo(magAmmount);
                }
                else
                {
                    result = _secondaryWeapon.RefillMaxAmmo(magAmmount);
                }
            }

            return result;
        }

        private void TogglePlayerKatana(bool toggle)
        {
            _animations.SetKatanaActive(toggle);
        }

        public void SpecialWeaponsHolster(float castTime, bool usesMelee)
        {
            StartCoroutine(HoldingWeapon(castTime, usesMelee));
        }

        public void HolsterWeapons()
        {
            ToggleWeapons(false);
            _currentWeapon = null;
            SetWeaponData(string.Empty, AmmoType.MELEE, 0, 0, 0);
        }

        private IEnumerator HoldingWeapon(float castTime, bool usesMelee)
        {
            var auxWeapon = _currentWeapon;
            HolsterWeapons();

            if (usesMelee)
            {
                DeployWeapon(_katana, forcedDeploy: true, isMelee: true);
            }

            yield return new WaitForSeconds(castTime);

            if (auxWeapon != null)
            {
                DeployWeapon(auxWeapon, forcedDeploy: true);
            }
        }
    }
}