using Keru.Scripts.Game.ScriptableObjects.Models;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Keru.Scripts.Game.ScriptableObjects
{
    [CreateAssetMenu]
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

        [Header("Level Data")]
        public List<SpecialLevel> SpecialLevels;
    }
}
