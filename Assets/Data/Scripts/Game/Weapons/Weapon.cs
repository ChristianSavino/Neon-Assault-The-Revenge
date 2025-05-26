using Keru.Scripts.Engine.Module;
using Keru.Scripts.Game.Entities;
using Keru.Scripts.Game.Entities.Player;
using Keru.Scripts.Game.ScriptableObjects;
using Keru.Scripts.Game.ScriptableObjects.Models;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace Keru.Scripts.Game.Weapons
{
    public class Weapon : MonoBehaviour
    {
        private GunStats _weaponData;
        private WeaponLevel _currentWeaponLevel;
        private WeaponThirdPersonModel _weaponModel;

        private Transform _shootPos;
        private Transform _magazineDrop;
        private Transform _bulletDrop;
        private Animator _muzzleFlash;

        private PlayerWeaponHandler _playerWeaponHandler;
        private AudioSource _audioSource;

        private int _currentBulletsInMag;

        private bool _canShoot;
        private bool _isReloading;
        private bool _canChangeWeapon;
        private float _nextShot;
        private bool _reloadCancel;
        private bool _isShooting;

        private float _recoil;
        private Camera _camera;
        private GameObject _owner;
        private GameObject _impact;
        private GameObject _casing;
        private Material _bulletTrail;
        private int _layerMask;


        public void SetConfig(PlayerWeaponHandler playerWeaponHandler, GameObject weaponModel, GameObject leftHand, int weaponLevel = 1, int currentBulletsInMag = 0, GameObject owner = null)
        {
            _playerWeaponHandler = playerWeaponHandler;

            var objects = weaponModel.transform.Find("Objects");

            var muzzleFlash = objects.transform.Find("MuzzleFlash");
            _shootPos = muzzleFlash.transform;
            _muzzleFlash = muzzleFlash.GetComponent<Animator>();
            _bulletDrop = objects.transform.Find("Bullet Drop");
            _magazineDrop = objects.transform.Find("MagDrop");

            _weaponModel = weaponModel.GetComponent<WeaponThirdPersonModel>();
            _weaponData = _weaponModel.WeaponData;
            _currentWeaponLevel = _weaponData.WeaponDataPerLevel[weaponLevel - 1];

            if (currentBulletsInMag != 0)
            {
                _currentBulletsInMag = currentBulletsInMag;
            }
            else
            {
                _currentBulletsInMag = _currentWeaponLevel.MagazineSize;
            }

            _audioSource = AudioManager.audioManager.CreateNewAudioSource(gameObject, Engine.SoundType.Effect);
            _camera = Camera.main;
            if (owner != null)
            {
                _owner = owner;
            }
            else
            {
                _owner = Player.Singleton.gameObject;
            }
            _layerMask = ~(1 << _owner.layer);
            _bulletTrail = CommonItemsManager.ItemsManager.BulletTrailDistortion;
            _weaponModel.ConfigWeapon(weaponLevel, leftHand.transform);
            _casing = GetCasing();
        }

        public void Deploy()
        {
            _canShoot = false;
            _isReloading = false;
            _canChangeWeapon = false;
            StartCoroutine(DeployWeapon());
        }

        public bool CanChangeWeapon()
        {
            return _canChangeWeapon;
        }

        private bool AbleToShoot()
        {
            return _canShoot && _currentBulletsInMag > 0 && !_isReloading;
        }

        public void StartsShoot()
        {
            if (AbleToShoot())
            {
                _isShooting = true;
            }
        }

        public void EndsShoot()
        {
            _isShooting = false;
        }

        public void Shoot(Vector3 direction, float damageMultiplier = 1, float fireRateMultiplier = 1)
        {
            if (AbleToShoot())
            {
                _isShooting = true;
                if (Time.time >= _nextShot)
                {
                    _nextShot = Time.time + 1f / (_currentWeaponLevel.FireRate * fireRateMultiplier);

                    WeaponShoot(damageMultiplier, direction);
                }
            }
            if (_isReloading && _weaponData.WeaponType == WeaponType.SHOTGUN && !_reloadCancel)
            {
                _reloadCancel = true;
            }
        }

        private void WeaponShoot(float damageMultiplier, Vector3 direction)
        {
            if (_weaponData.WeaponType == WeaponType.SHOTGUN)
            {
                _recoil = _currentWeaponLevel.MaxRecoil;

                for (int i = 0; i < 12; i++)
                {
                    CreateBullet(damageMultiplier, GetDirectionRecoil(direction));
                }
            }
            else
            {
                if (_weaponData.Projectile == null)
                {
                    CreateBullet(damageMultiplier, GetDirectionRecoil(direction));
                }
                else
                {

                }
            }

            PlayAnimation(WeaponActions.SHOOT);
            _muzzleFlash.Play("Shoot");
            _audioSource.PlayOneShot(_currentWeaponLevel.ShootSound);
            _currentBulletsInMag--;
            CalculateRecoilAmmount(_currentWeaponLevel.RecoilPerShot);
            StartCasingSpawn();
        }

        private void CalculateRecoilAmmount(float ammountToSum)
        {
            _recoil += ammountToSum;
            _recoil = Mathf.Clamp(_recoil, 0, _currentWeaponLevel.MaxRecoil);
        }

        private void StartCasingSpawn()
        {
            if(_casing != null)
            {
                StartCoroutine(CreateCasing());
            }
        }

        private IEnumerator CreateCasing()
        {
            yield return new WaitForSeconds(_weaponData.CasingSpawnDelay);
            var casing = Instantiate(_casing, _bulletDrop.position, _bulletDrop.transform.rotation);
            Destroy(casing, 5f);
        }

        private void CreateBullet(float damageMultiplier, Vector3 direction)
        {
            RaycastHit hit;
            if (Physics.Raycast(_shootPos.position, direction, out hit, 1000f, _layerMask))
            {
                CreateLine(hit.point);
                var entity = hit.transform.GetComponent<Entity>();
                if (entity != null)
                {
                    var damage = damageMultiplier * _currentWeaponLevel.Damage;
                    entity.OnDamagedUnit(Mathf.RoundToInt(damage), hit.point, _owner, DamageType.BULLET, _weaponData.Force);
                }
            }
        }

        private void CreateLine(Vector3 hit)
        {
            var line = new GameObject("Line");
            line = Instantiate(line, _bulletDrop.position, _bulletDrop.rotation);
           
            var lineRenderer = line.AddComponent<LineRenderer>();
            lineRenderer.material = _bulletTrail;
            lineRenderer.startWidth = 0f;
            lineRenderer.endWidth = 0.05f;
            lineRenderer.shadowCastingMode = ShadowCastingMode.Off;
            lineRenderer.textureMode = LineTextureMode.Tile;
            lineRenderer.SetPosition(0, _shootPos.position);
            lineRenderer.SetPosition(1, hit);
            
            Destroy(line, 0.1f);
        }

        private Vector3 GetDirectionRecoil(Vector3 direction)
        {
            var recoil = _recoil / 100;

            direction += new Vector3(
                Random.Range(-recoil, recoil),
                Random.Range(-recoil, recoil),
                Random.Range(-recoil, recoil));

            return direction;
        }

        public void Reload()
        {
            if (_currentBulletsInMag < _currentWeaponLevel.MagazineSize && !_isReloading)
            {
                _canShoot = false;
                _isReloading = true;
                _canChangeWeapon = false;

                if (_weaponData.WeaponType != WeaponType.SHOTGUN)
                {
                    if (_currentBulletsInMag == 0)
                    {
                        PlayAnimation(WeaponActions.RELOAD_EMPTY);
                        _audioSource.PlayOneShot(_weaponData.EmptyReloadSound);
                        StartCoroutine(ReloadWeapon(true));
                    }
                    else
                    {
                        PlayAnimation(WeaponActions.RELOAD);
                        _audioSource.PlayOneShot(_weaponData.ReloadSound);
                        StartCoroutine(ReloadWeapon(false));
                    }
                    StartCoroutine(DropMagazine());
                }
                else
                {
                    StartCoroutine(ReloadShotgun());
                }
            }
        }

        private IEnumerator ReloadWeapon(bool isEmpty)
        {
            _weaponModel.PlayReloadAnimation();
            yield return new WaitForSeconds(isEmpty ? _weaponData.ReloadTimeEmpty : _weaponData.ReloadTime);
            _currentBulletsInMag = _currentWeaponLevel.MagazineSize;

            _isReloading = false;
            _canShoot = true;
            _canChangeWeapon = true;
        }

        private IEnumerator ReloadShotgun()
        {
            PlayAnimation(WeaponActions.RELOAD_OPEN);
            _audioSource.PlayOneShot(_weaponData.BeginReload);

            yield return new WaitForSeconds(0.5f);

            while (!_reloadCancel && _currentBulletsInMag < _currentWeaponLevel.MagazineSize)
            {
                PlayAnimation(WeaponActions.RELOAD_INSERT);
                _audioSource.PlayOneShot(_weaponData.ReloadSound);
                yield return new WaitForSeconds(_weaponData.ReloadTime);
                _currentBulletsInMag++;
            }

            PlayAnimation(WeaponActions.RELOAD_CLOSE);
            _audioSource.PlayOneShot(_weaponData.FinishReload);

            yield return new WaitForSeconds(0.5f);

            _canChangeWeapon = true;
            _reloadCancel = false;
            _isReloading = false;
            _canShoot = true;
        }

        private IEnumerator DropMagazine()
        {
            if (_weaponData.MagazineToDrop != null)
            {
                yield return new WaitForSeconds(_weaponData.MagSpawn);
                var mag = Instantiate(_weaponData.MagazineToDrop, _magazineDrop.position, _magazineDrop.rotation).GetComponent<Renderer>();
                mag.gameObject.AddComponent<BoxCollider>();
                mag.gameObject.AddComponent<Rigidbody>();
                mag.enabled = true;

                Destroy(mag.gameObject, 60f);
            }
        }

        private IEnumerator DeployWeapon()
        {
            _audioSource.PlayOneShot(_weaponData.DrawSound);
            PlayAnimation(WeaponActions.DEPLOY);

            yield return new WaitForSeconds(0.5f);
            _canChangeWeapon = true;
            _canShoot = true;
            StartCoroutine(RecoilRecover());
        }

        private void PlayAnimation(WeaponActions weaponAction)
        {
            _playerWeaponHandler.PlayAnimation(weaponAction, _weaponData.WeaponCode);
        }

        public void Die()
        {
            _canShoot = false;
            _isReloading = false;
            _canChangeWeapon = false;
            gameObject.AddComponent<BoxCollider>();
            gameObject.AddComponent<Rigidbody>();

            StartCoroutine(WeaponSeparation());
        }

        private IEnumerator WeaponSeparation()
        {
            yield return new WaitForSeconds(0.5f);
            gameObject.transform.parent = null;
            this.enabled = false;
        }

        private IEnumerator RecoilRecover()
        {
            yield return new WaitForSeconds(0.1f);
            if (!_isShooting)
            {
                CalculateRecoilAmmount(-_currentWeaponLevel.RecoilRecover/5);
            }
            StartCoroutine(RecoilRecover());
        }

        private GameObject GetCasing()
        {
            switch (_currentWeaponLevel.AmmoType)
            {
                case AmmoType.MM9:
                case AmmoType.CAL45:
                    return CommonItemsManager.ItemsManager.SmallBulletCasing;
                case AmmoType.G12:
                    return CommonItemsManager.ItemsManager.ShellCasing;
                case AmmoType.CAL556:
                case AmmoType.CAL762:
                case AmmoType.CAL50:
                    return CommonItemsManager.ItemsManager.BigBulletCasing;
            }

            return null;
        }
    }
}
