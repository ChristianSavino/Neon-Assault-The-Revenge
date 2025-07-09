using Keru.Scripts.Game.Entities.Player.UI;
using Keru.Scripts.Game.ScriptableObjects;
using Keru.Scripts.Game.Specials;
using Keru.Scripts.Game.Specials.Overrides;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

namespace Keru.Scripts.Game.Entities.Player
{
    public class PlayerSpecialHandler : MonoBehaviour
    {
        [SerializeField] private List<SpecialStats> _allSpecials;
        [SerializeField] private GameObject _container;

        private Special _ultimate;
        private Special _secondary;

        private Dictionary<string, KeyCode> _keys;

        private PlayerWeaponHandler _weaponHandler;
        private PlayerMovement _movement;
        private PlayerThirdPersonAnimations _animations;
        private SpecialUIHandler _specialUiHandler;

        public void SetConfig(SaveGameFile saveGame, Dictionary<string, KeyCode> keys, PlayerMovement playerMovement, PlayerWeaponHandler weaponHandler, PlayerThirdPersonAnimations thirdPersonAnimations, SpecialUIHandler specialUIHandler)
        {
            _keys = keys;
            _movement = playerMovement;
            _weaponHandler = weaponHandler;
            _animations = thirdPersonAnimations;

            _specialUiHandler = specialUIHandler;
            _specialUiHandler.SetConfig(_keys);

            var ultimateSkill = saveGame.CurrentCharacterData.UltimateSkill;
            _ultimate = GetCorrectSpecial(ultimateSkill);
            _ultimate.SetConfig(_animations, _allSpecials.First(x => x.AbilityCode == ultimateSkill), saveGame.UnlockedSkills.First(x => x.Code == ultimateSkill).Level, gameObject);
            _specialUiHandler.SetConfigSpecial(_ultimate);

            var secondarySkill = saveGame.CurrentCharacterData.SecondarySkill;
            _secondary = GetCorrectSpecial(secondarySkill);
            _secondary.SetConfig(_animations, _allSpecials.First(x => x.AbilityCode == secondarySkill), saveGame.UnlockedSkills.First(x => x.Code == secondarySkill).Level, gameObject);
            _specialUiHandler.SetConfigSpecial(_secondary);

        }

        private void Update()
        {
            if (Input.GetKeyDown(_keys["Second"]) && _secondary != null)
            {
                CastSpecial(_secondary);
            }
            else if (Input.GetKeyDown(_keys["Special"]) && _ultimate != null)
            {
                CastSpecial(_ultimate);
            }
        }

        private void CastSpecial(Special special)
        {
            var casted = special.Execute();
            if (casted)
            {
                var stats = special.GetStats();
                _animations.PlaySpecialAnimation(stats.name, stats.CastTime);
                SetPlayerControls(false);
                StartCoroutine(ResetPlayer(stats.CastTime, stats.UsesMelee));
            }
        }

        private Special GetCorrectSpecial(AbilityCodes abilityCode)
        {
            switch (abilityCode)
            {
                case AbilityCodes.BULLETTIME:
                    return _container.AddComponent<BulletTime>();
                case AbilityCodes.JUDGEMENTCUT:
                    return _container.AddComponent<JudgementCut>();
                default:
                    return null;
            }
        }

        private IEnumerator ResetPlayer(float castTime, bool usesKatana)
        {
            _weaponHandler.SpecialWeaponsHolster(castTime, usesKatana);
            yield return new WaitForSeconds(castTime);
            SetPlayerControls(true);
        }

        public void SetPlayerControls(bool toggle)
        {
            _weaponHandler.CanInteract = toggle;
            _movement.CanMove = toggle;
        }

        public void Die()
        {
            if (_secondary != null)
            {
                _secondary.Die();
            }
            if (_ultimate != null)
            {
                _ultimate.Die();
            }
            enabled = false;
        }
    }
}