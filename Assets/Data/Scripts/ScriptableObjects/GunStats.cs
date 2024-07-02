using Keru.Scripts.Game.ScriptableObjects.Models;
using System.Collections.Generic;
using UnityEngine;

namespace Keru.Scripts.Game.ScriptableObjects
{
    [CreateAssetMenu]
    public class GunStats : ScriptableObject
    {
        [Header("Base Parameters")]
        public WeaponCodes WeaponCode;
        public WeaponSlot WeaponSlot;
        public WeaponType WeaponType;
        public List<WeaponLevel> WeaponDataPerLevel;
        public float HeadShotMultiplier = 2;
        public float Force;

        [Header("Extras")]
        public GameObject Projectile;
        public GameObject MagazineToDrop;
        public float CasingSpawnDelay;
        public float BulletSpawn;

        [Header("Sound")]
        public AudioClip ReloadSound;
        public AudioClip EmptyReloadSound;
        public AudioClip DrawSound;
        public AudioClip HolsterSound;

        [Header("Shotgun Shound")]
        public AudioClip BeginReload;
        public AudioClip FinishReload;
    }
}