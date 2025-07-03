using UnityEngine;
using UnityEngine.UI;

namespace Keru.Scripts.Game.Entities.Player.UI
{
    public class WeaponUIHandler : MonoBehaviour
    {
        [SerializeField] private GameObject _weaponBlock;
        [SerializeField] private Text _name, _munitionType, _bulletsCounter;
        [SerializeField] private Image _bulletsInMag;

        private int _maxBulletsInMag;
        private bool _isWeaponSet = false;

        private void Start()
        {
            if(!_isWeaponSet)
            {
                _weaponBlock.SetActive(false);
            }       
        }

        public void SetWeapon(string name, AmmoType munitionType, int bulletsInMag, int maxBulletsInMag, int totalBullets)
        {
            _weaponBlock.SetActive(true);
            _maxBulletsInMag = maxBulletsInMag;

            _name.text = name.ToUpper();
            SetWeaponMunitionType(munitionType);
            if(munitionType == AmmoType.MELEE)
            {
                _bulletsCounter.text = "INF";
                _bulletsInMag.fillAmount = 1f;
                _munitionType.text = "MELEE";
            }
            else
            {
                UpdateBullets(bulletsInMag, totalBullets);
            }              
        }

        private void SetWeaponMunitionType(AmmoType ammoType)
        {
            var munitionType = "";
            var color = Color.white;

            switch (ammoType)
            {
                case AmmoType.MM9:
                    munitionType = "9mm";
                    color = Color.white;
                    break;
                case AmmoType.G12:
                    munitionType = "12G";
                    color = Color.red;
                    break;
                case AmmoType.CAL45:
                    munitionType = ".45";
                    color = Color.blue;
                    break;
                case AmmoType.CAL556:
                    munitionType = "5.56";
                    color = Color.cyan;
                    break;
                case AmmoType.CAL762:
                    munitionType = "7.62";
                    color = Color.cyan;
                    break;
                case AmmoType.CAL50:
                    munitionType = ".50";
                    color = Color.blue;
                    break;
                case AmmoType.ROCKET:
                    munitionType = "HER";
                    color = Color.yellow;
                    break;
                case AmmoType.GRENADE:
                    munitionType = "HEG";
                    color = Color.yellow;
                    break;
                case AmmoType.SPECIAL:
                    munitionType = "SPEC";
                    color = Color.magenta;
                    break;

            }

            _munitionType.text = munitionType.ToUpper();
            _munitionType.color = color;
        }
        public void UpdateBullets(int bulletsInMag, int totalBullets)
        {
            _bulletsCounter.text = $"{bulletsInMag}|{totalBullets}";
            _bulletsInMag.fillAmount = (float)bulletsInMag / _maxBulletsInMag;

            if (bulletsInMag <= 0 && totalBullets <= 0)
            {
                _bulletsCounter.color = Color.red;
            }
            else
            {
                _bulletsCounter.color = Color.white;
            }
        }
    }
}