using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Keru.Scripts.Game.ScriptableObjects
{
    [CreateAssetMenu]
    public class PassiveStats : ScriptableObject
    {
        public PassiveCode Code;
        public PassivePotivity Potivity;
        public string Description;
        public Sprite Icon;
        public float Duration;
        public GameObject Effects;
        public bool IsMeshParticle;
        public bool UsesRealTime;
    }
}