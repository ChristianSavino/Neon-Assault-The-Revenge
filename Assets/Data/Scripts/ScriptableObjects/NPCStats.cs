using System.Collections.Generic;
using UnityEngine;

namespace Keru.Scripts.Game.ScriptableObjects
{
    [CreateAssetMenu(fileName = "EnemyStats", menuName = "ScriptableObjects/EnemyStats", order = 2)]
    public class NPCStats : ScriptableObject
    {
        [Header("Base Parameters")]
        public NPCType NPCType;
        public int MaxLife = 100;
        public float MaxSpeed = 8f;
        public List<WeaponCodes> PrimaryWeaponCodes = new List<WeaponCodes>();
        public int PrimaryWeaponLevel = 1;
        public List<WeaponCodes> SecondaryWeaponCodes = new List<WeaponCodes>();
        public int SecondaryWeaponLevel = 1;

        [Header("Combat Parameters")]
        public float ViewDistance = 30f;
        public float AttackDistance = 15f;
        public float AngleVision = 45f;

        [Header("Audio")]
        public List<AudioClip> CombatClips = new List<AudioClip>();
        public List<AudioClip> DeathClips = new List<AudioClip>();
        public List<AudioClip> PainClips = new List<AudioClip>();
    }
}

