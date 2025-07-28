    using Keru.Scripts.Game.Entities.Player.UI;
using Keru.Scripts.Game.ScriptableObjects;
using Keru.Scripts.Game.Specials;
using Keru.Scripts.Game.Specials.Overrides;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        private Coroutine _resetCoroutine;

        public bool CanCast { get; set; } = true;

        public void SetConfig(
            SaveGameFile saveGame,
            Dictionary<string, KeyCode> keys,
            PlayerMovement playerMovement,
            PlayerWeaponHandler weaponHandler,
            PlayerThirdPersonAnimations thirdPersonAnimations,
            SpecialUIHandler specialUIHandler)
        {
            _keys = keys;
            _movement = playerMovement;
            _weaponHandler = weaponHandler;
            _animations = thirdPersonAnimations;
            _specialUiHandler = specialUIHandler;
            _specialUiHandler.SetConfig(_keys);

            _ultimate = ConfigureSpecial(
                saveGame.CurrentCharacterData.UltimateSkill,
                saveGame,
                AbilitySlot.ULTIMATE);

            _secondary = ConfigureSpecial(
                saveGame.CurrentCharacterData.SecondarySkill,
                saveGame,
                AbilitySlot.SECONDARY);
        }

        private Special ConfigureSpecial(AbilityCodes abilityCode, SaveGameFile saveGame, AbilitySlot slot)
        {
            var stats = _allSpecials.FirstOrDefault(x => x.AbilityCode == abilityCode);
            var unlocked = saveGame.UnlockedSkills.FirstOrDefault(x => x.Code == abilityCode);

            if (stats == null || unlocked == null)
                return null;

            var special = GetOrAddSpecialComponent(abilityCode);
            special.SetConfig(_animations, stats, unlocked.Level, gameObject);
            _specialUiHandler.SetConfigSpecial(special);
            return special;
        }

        private Special GetOrAddSpecialComponent(AbilityCodes abilityCode)
        {
            switch (abilityCode)
            {
                case AbilityCodes.BULLETTIME:
                    return _container.GetComponent<BulletTime>() ?? _container.AddComponent<BulletTime>();
                case AbilityCodes.JUDGEMENTCUT:
                    return _container.GetComponent<JudgementCut>() ?? _container.AddComponent<JudgementCut>();
                default:
                    return null;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(_keys["Second"]) && _secondary != null && CanCast)
            {
                CastSpecial(_secondary);
            }
            else if (Input.GetKeyDown(_keys["Special"]) && _ultimate != null && CanCast)
            {
                CastSpecial(_ultimate);
            }
        }

        private void CastSpecial(Special special)
        {
            if (special == null) return;

            var casted = special.Execute();
            if (casted)
            {
                var stats = special.GetStats();
                _animations.PlaySpecialAnimation(stats.name, stats.CastTime);
                SetPlayerControls(false);

                if (_resetCoroutine != null)
                    StopCoroutine(_resetCoroutine);

                _resetCoroutine = StartCoroutine(ResetPlayer(stats.CastTime, stats.UsesMelee));
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
            CanCast = toggle;
        }

        public void Die()
        {
            _secondary?.Die();
            _ultimate?.Die();
            enabled = false;
        }
    }
}