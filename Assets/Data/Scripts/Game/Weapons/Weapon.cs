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
        protected GunStats _weaponData;
        protected WeaponLevel _currentWeaponLevel;
        protected WeaponThirdPersonModel _weaponModel;

        private Transform _shootPos;
        private Transform _magazineDrop;
        private Transform _bulletDrop;
        private Animator _muzzleFlash;

        protected PlayerWeaponHandler _playerWeaponHandler;
        protected AudioSource _audioSource;

        protected int _currentBulletsInMag;
        private int _maxTotalBullets;
        private int _currentTotalBullets;

        protected bool _canShoot;
        private bool _isReloading;
        protected bool _canChangeWeapon;
        private float _nextShot;
        private bool _reloadCancel;
        private bool _isShooting;

        private float _recoil;
        protected GameObject _owner;
        private GameObject _impact;
        private GameObject _casing;
        private Material _bulletTrail;
        private int _layerMask;


        private Coroutine _recoilCoroutine;

        public virtual void SetConfig(PlayerWeaponHandler playerWeaponHandler, GameObject weaponModel, GameObject leftHand, int weaponLevel = 1, int currentBulletsInMag = 0, int currentTotalBullets = 0, GameObject owner = null)
        {
            _playerWeaponHandler = playerWeaponHandler;

            var objects = weaponModel.transform.Find("Objects");
            var muzzleFlash = objects?.Find("MuzzleFlash");
            _shootPos = muzzleFlash?.transform;
            _muzzleFlash = muzzleFlash?.GetComponent<Animator>();
            _bulletDrop = objects?.Find("Bullet Drop");
            _magazineDrop = objects?.Find("MagDrop");

            _weaponModel = weaponModel.GetComponent<WeaponThirdPersonModel>();
            _weaponData = _weaponModel.WeaponData;
            _currentWeaponLevel = _weaponData.WeaponDataPerLevel[weaponLevel - 1];

            _currentBulletsInMag = currentBulletsInMag != 0 ? currentBulletsInMag : _currentWeaponLevel.MagazineSize;

            _audioSource = AudioManager.audioManager.CreateNewAudioSource(gameObject, Engine.SoundType.Effect);
            _owner = owner ?? Player.Singleton.gameObject;
            _layerMask = ~(1 << _owner.layer);
            _bulletTrail = CommonItemsManager.ItemsManager.BulletTrailDistortion;
            _weaponModel.ConfigWeapon(weaponLevel, leftHand.transform);
            _casing = GetCasing();

            _maxTotalBullets = _currentWeaponLevel.MagazineSize * (2 + weaponLevel);
            _currentTotalBullets = currentTotalBullets != 0 ? currentTotalBullets : _maxTotalBullets;
            _audioSource.clip = _currentWeaponLevel.ShootSound;
        }

        public virtual void Deploy()
        {
            _canShoot = false;
            _isReloading = false;
            _canChangeWeapon = false;
            StartCoroutine(DeployWeapon());
            SetWeaponData();
        }

        protected void SetWeaponData()
        {
            if (_playerWeaponHandler != null)
            {
                _playerWeaponHandler.SetWeaponData(_weaponData.name, _currentWeaponLevel.AmmoType, _currentBulletsInMag, _currentWeaponLevel.MagazineSize, _currentTotalBullets);
            }
        }

        protected void UpdateWeaponData()
        {
            if (_playerWeaponHandler != null)
            {
                _playerWeaponHandler.UpdateWeaponData(_currentBulletsInMag, _currentTotalBullets);
            }
        }

        public bool CanChangeWeapon()
        {
            return _canChangeWeapon;
        }

        private bool AbleToShoot()
        {
            return _canShoot && _currentBulletsInMag > 0 && !_isReloading;
        }

        public virtual void StartsShoot()
        {
            if (AbleToShoot())
            {
                _isShooting = true;
            }
        }

        public virtual void EndsShoot()
        {
            _isShooting = false;
        }

        public virtual void Shoot(Vector3 direction, float damageMultiplier = 1, float fireRateMultiplier = 1)
        {
            if (AbleToShoot())
            {
                _isShooting = true;
                if (Time.time >= _nextShot)
                {
                    _nextShot = Time.time + 1f / (_currentWeaponLevel.FireRate * fireRateMultiplier);

                    WeaponShoot(damageMultiplier, direction);
                    UpdateWeaponData();
                }
            }
            else if (_isReloading && _weaponData.WeaponType == WeaponType.SHOTGUN && !_reloadCancel)
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
                    CreateBulletObject(damageMultiplier, GetDirectionRecoil(direction));
                }
            }

            PlayAnimation(WeaponActions.SHOOT);
            if (!_currentWeaponLevel.IsSilenced)
            {
                _muzzleFlash.Play("Shoot");
            }
            _audioSource.Play();
            _currentBulletsInMag--;
            CalculateRecoilAmmount(_currentWeaponLevel.RecoilPerShot);
            StartCasingSpawn();

            if (_currentBulletsInMag == 0)
            {
                _weaponModel.TurnBulletModel(false);
            }
        }

        private void CalculateRecoilAmmount(float ammountToSum)
        {
            _recoil += ammountToSum;
            _recoil = Mathf.Clamp(_recoil, 0, _currentWeaponLevel.MaxRecoil);
        }

        private void StartCasingSpawn()
        {
            if (_casing != null)
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
                var entity = hit.transform.GetComponentInParent<Entity>();
                if (entity != null)
                {
                    var damage = damageMultiplier * _currentWeaponLevel.Damage;
                    entity.OnDamagedUnit(Mathf.RoundToInt(damage), hit.point, _owner, DamageType.BULLET, _weaponData.Force);
                }
            }
        }

        private void CreateBulletObject(float damageMultiplier, Vector3 direction)
        {
            var bullet = Instantiate(_weaponData.Projectile, _muzzleFlash.transform.position, Quaternion.identity);
            bullet.transform.forward = direction;
            bullet.layer = _owner.layer;
            var bulletData = bullet.GetComponent<Bullet>();
            bulletData.SetUp(
                Mathf.RoundToInt(damageMultiplier * _currentWeaponLevel.Damage),
                _weaponData.Force,
                _owner);
        }

        private void CreateLine(Vector3 hit)
        {
            var line = new GameObject("Line");

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

        public virtual void Reload()
        {
            if (_currentBulletsInMag < _currentWeaponLevel.MagazineSize && !_isReloading && _currentTotalBullets > 0)
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
            _weaponModel.PlayReloadAnimation(isEmpty);
            var bulletsToRefill = _currentWeaponLevel.MagazineSize;

            yield return new WaitForSeconds(isEmpty ? _weaponData.ReloadTimeEmpty : _weaponData.ReloadTime);

            if (_currentTotalBullets < _currentWeaponLevel.MagazineSize)
            {
                bulletsToRefill = _currentTotalBullets;
                _currentTotalBullets = 0;
            }
            else
            {
                _currentTotalBullets -= _currentWeaponLevel.MagazineSize - _currentBulletsInMag;
            }

            _currentBulletsInMag = bulletsToRefill;

            _isReloading = false;
            _canShoot = true;
            _canChangeWeapon = true;
            UpdateWeaponData();
        }

        private IEnumerator ReloadShotgun()
        {
            PlayAnimation(WeaponActions.RELOAD_OPEN);
            _audioSource.PlayOneShot(_weaponData.BeginReload);

            yield return new WaitForSeconds(0.5f);

            while (!_reloadCancel && _currentBulletsInMag < _currentWeaponLevel.MagazineSize && _currentTotalBullets > 0)
            {
                PlayAnimation(WeaponActions.RELOAD_INSERT);
                _audioSource.PlayOneShot(_weaponData.ReloadSound);
                yield return new WaitForSeconds(_weaponData.ReloadTime);
                _currentBulletsInMag++;
                _currentTotalBullets--;
                UpdateWeaponData();
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

            if (_recoilCoroutine != null)
            {
                StopCoroutine(_recoilCoroutine);
            }
                
            _recoilCoroutine = StartCoroutine(RecoilRecover());
        }

        protected virtual void PlayAnimation(WeaponActions weaponAction)
        {
            if (_playerWeaponHandler != null)
            {
                _playerWeaponHandler.PlayAnimation(weaponAction, _weaponData.WeaponCode);
            }
        }

        public void Die()
        {
            _canShoot = false;
            _isReloading = false;
            _canChangeWeapon = false;
            Destroy(_audioSource);

            StartCoroutine(WeaponSeparation());
        }

        private IEnumerator WeaponSeparation()
        {
            yield return new WaitForSeconds(0.5f);
            gameObject.AddComponent<BoxCollider>();
            var rb = gameObject.AddComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            gameObject.transform.parent = null;
            this.enabled = false;
        }

        private IEnumerator RecoilRecover()
        {
            while (enabled)
            {
                yield return new WaitForSeconds(0.1f);
                if (!_isShooting)
                {
                    CalculateRecoilAmmount(-_currentWeaponLevel.RecoilRecover / 5);
                }
            }
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

        public int GetMagazineSize()
        {
            return _currentWeaponLevel.MagazineSize;
        }

        public int GetCurrentBulletsInMag()
        {
            return _currentBulletsInMag;
        }

        public bool RefillMaxAmmo(float magazine)
        {
            if (_currentTotalBullets >= _maxTotalBullets)
            {
                return false;
            }

            var ammoToRefill = Mathf.RoundToInt(magazine * _currentWeaponLevel.MagazineSize);

            if (ammoToRefill + _currentTotalBullets >= _maxTotalBullets)
            {
                _currentTotalBullets = _maxTotalBullets;
            }
            else
            {
                _currentTotalBullets += ammoToRefill;
            }

            if(gameObject.activeSelf)
            {
                UpdateWeaponData();
            }

            return true;
        }
    }
}
