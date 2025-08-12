using Keru.Scripts.Game.ScriptableObjects.Models;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Keru.Scripts.Game.ScriptableObjects
{
    [CreateAssetMenu(fileName = "SpecialStats", menuName = "ScriptableObjects/SpecialStats", order = 4)]
    public class SpecialStats : ScriptableObject
    {
        [Header("Base Stats")]
        public AbilityCodes AbilityCode;
        public AbilitySlot AbilitySlot;
        public GameObject SpecialEffects;
        public GameObject SpecialObject;
        public VolumeProfile Volume;
        public Sprite Icon;
        public float CastTime;
        public AudioClip SoundEffect;
        public AudioClip EndSoundEffect;
        public bool UsesMelee;

        [Header("Level Data")]
        public List<SpecialLevel> SpecialLevels;

        [Header("Passives")]
        public PassiveStats Passive;
    }
}
