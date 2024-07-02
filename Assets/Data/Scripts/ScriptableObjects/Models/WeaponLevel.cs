using System;
using UnityEngine;

namespace Keru.Scripts.Game.ScriptableObjects.Models
{
    [Serializable]
    public class WeaponLevel
    {
        public int Damage;
        public AmmoType AmmoType;
        public int MagazineSize;
        public float FireRate;
        public float RecoilPerShot;
        public float MaxRecoil;
        public float RecoilRecover;
        public string Attachments;
        public AudioClip ShootSound;
        public bool IsSilenced;
    }
}