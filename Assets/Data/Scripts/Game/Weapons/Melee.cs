using Keru.Scripts.Engine.Module;
using Keru.Scripts.Game.Effects.Trails;
using Keru.Scripts.Game.Entities;
using Keru.Scripts.Game.Entities.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Keru.Scripts.Game.Weapons
{
    public class Melee : Weapon
    {
        private SwordTrailRenderer _trailRenderer;
        private List<Entity> _alreadyHitEntities = new List<Entity>();
        private float _currentDamageMultiplier = 1f;

        public override void SetConfig(PlayerWeaponHandler playerWeaponHandler, GameObject weaponModel, GameObject leftHand, int weaponLevel = 1, int currentBulletsInMag = 0, int currentTotalBullets = 0, GameObject owner = null)
        {
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
            _currentBulletsInMag = 1;
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
            if(_canShoot)
            {
                _currentDamageMultiplier = damageMultiplier;
                _canChangeWeapon = false;
                _canShoot = false;
                StartCoroutine(AttackRoutine());
            }
        }

        IEnumerator AttackRoutine()
        {
            var enableTime = _currentWeaponLevel.FireRate * 0.3333f;
            var activeTime = _currentWeaponLevel.FireRate * 0.6666f;

            PlayAnimation(WeaponActions.SHOOT);
            _alreadyHitEntities.Clear();

            yield return new WaitForSeconds(enableTime);

            _trailRenderer.Toggle(true);

            var slashActiveTime = activeTime / 5f;
            for (int i = 0; i < 5; i++)
            {
                DetectHits();
                yield return new WaitForSeconds(slashActiveTime);
            }

            _trailRenderer.Toggle(false);

            yield return new WaitForSeconds(_currentWeaponLevel.FireRate - enableTime - activeTime);

            _canChangeWeapon = true;
            _canShoot = true;
        }


        private void DetectHits()
        {
            var distance = 1f;
            float radius = 1f;

            var center = transform.position + transform.forward * distance;

            var hits = Physics.OverlapSphere(center, radius);

            foreach (var hit in hits)
            {
                if(hit.gameObject == _owner)
                {
                    continue;
                }
                
                var entity = hit.GetComponentInParent<Entity>();
                if (entity != null && !_alreadyHitEntities.Contains(entity))
                {
                    _alreadyHitEntities.Add(entity);

                    entity.OnDamagedUnit(
                        Mathf.RoundToInt(_currentWeaponLevel.Damage * _currentDamageMultiplier),
                        hit.ClosestPoint(transform.position),
                        _owner,
                        DamageType.MELEE,
                        10f
                    );
                }
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