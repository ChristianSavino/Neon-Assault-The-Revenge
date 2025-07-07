using Keru.Scripts.Engine.Module;
using Keru.Scripts.Game.Effects.Trails;
using Keru.Scripts.Game.Entities;
using Keru.Scripts.Game.Entities.Player;
using System.Collections;
using UnityEngine;

namespace Keru.Scripts.Game.Weapons
{
    public class Melee : Weapon
    {
        private SwordTrailRenderer _trailRenderer;

        private float _currentDamageMultiplier = 1f;
        private Collider _collider;
        private Coroutine _attackRoutine;

        public override void SetConfig(PlayerWeaponHandler playerWeaponHandler, GameObject weaponModel, GameObject leftHand, int weaponLevel = 1, int currentBulletsInMag = 0, int currentTotalBullets = 0, GameObject owner = null)
        {
            _collider = gameObject.GetComponent<Collider>();
            _collider.enabled = false;
            _playerWeaponHandler = playerWeaponHandler;
            _weaponModel = weaponModel.GetComponent<WeaponThirdPersonModel>();
            _weaponData = _weaponModel.WeaponData;
            _currentWeaponLevel = _weaponData.WeaponDataPerLevel[weaponLevel - 1];

            _audioSource = AudioManager.audioManager.CreateNewAudioSource(gameObject, Engine.SoundType.Effect);

            if (owner != null)
            {
                _owner = owner;
            }
            else
            {
                _owner = Player.Singleton.gameObject;
            }

            _playerWeaponHandler.SetWeaponData(_weaponData.name, AmmoType.MELEE, 0, 0, 0);
            _trailRenderer = GetComponentInChildren<SwordTrailRenderer>();
        }

        public override void Deploy()
        {
            base.Deploy();
        }

        public override void StartsShoot()
        {
            base.StartsShoot();
        }

        public override void EndsShoot()
        {
            base.EndsShoot();
        }

        public override void Reload()
        {

        }

        public override void Shoot(Vector3 direction, float damageMultiplier = 1, float fireRateMultiplier = 1)
        {
            _currentDamageMultiplier = damageMultiplier;
            _canChangeWeapon = false;
            if (_attackRoutine == null)
            {
                _attackRoutine = StartCoroutine(AttackRoutine());
            }
        }

        IEnumerator AttackRoutine()
        {
            var enableTime = _currentWeaponLevel.FireRate * 0.3333f;
            var activeTime = _currentWeaponLevel.FireRate * 0.6666f;

            PlayAnimation(WeaponActions.SHOOT);

            yield return new WaitForSeconds(enableTime);

            _collider.enabled = true;
            _trailRenderer.Toggle(true);

            yield return new WaitForSeconds(activeTime);

            _collider.enabled = false;
            _trailRenderer.Toggle(false);

            yield return new WaitForSeconds(_currentWeaponLevel.FireRate - enableTime - activeTime);

            _attackRoutine = null;
            _canChangeWeapon = true;
        }

        private void OnCollisionEnter(Collision collision)
        {
            var entity = collision.gameObject.GetComponent<Entity>();

            if (entity != null)
            {
                entity.OnDamagedUnit(Mathf.RoundToInt(_currentWeaponLevel.Damage * _currentDamageMultiplier), collision.contacts[0].point, _owner, DamageType.MELEE, 10f);
            }
        }

        protected override void PlayAnimation(WeaponActions weaponAction)
        {
            if (_playerWeaponHandler != null)
            {
                _playerWeaponHandler.PlayMeleeAnimation(weaponAction, _weaponData.WeaponCode);
            }
        }
    }
}