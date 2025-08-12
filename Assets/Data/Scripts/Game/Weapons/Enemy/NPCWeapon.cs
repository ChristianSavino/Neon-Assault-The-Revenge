using Keru.Scripts.Engine.Module;
using Keru.Scripts.Game.Entities;
using Keru.Scripts.Game.Entities.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Keru.Scripts.Game.Weapons.NPCs
{
    public class NPCWeapon : Weapon
    {      
        public override void SetConfig(WeaponHandler weaponHandler, GameObject weaponModel, GameObject leftHand, int weaponLevel = 1, int currentBulletsInMag = 0, int currentTotalBullets = 0, GameObject owner = null)
        {
            base.SetConfig(weaponHandler, weaponModel, leftHand, weaponLevel, currentBulletsInMag, currentTotalBullets, owner);
            
            if(_weaponData.Projectile == null)
            {
                SetBullet(CommonItemsManager.ItemsManager.EnemyNormalBullet);
            }
        }

        public override void Deploy()
        {
            base.Deploy();
        }

        public override void EndsShoot()
        {
            base.EndsShoot();
        }

        public override void StartsShoot()
        {
            base.StartsShoot();
        }

        public override void Shoot(Vector3 direction, float damageMultiplier = 1, float fireRateMultiplier = 1)
        {
            base.Shoot(direction, damageMultiplier, fireRateMultiplier);
        }

        public override void Reload()
        {
            base.Reload();
        }

        protected override void PlayAnimation(WeaponActions weaponAction)
        {
            base.PlayAnimation(weaponAction);
        }
    }
}
