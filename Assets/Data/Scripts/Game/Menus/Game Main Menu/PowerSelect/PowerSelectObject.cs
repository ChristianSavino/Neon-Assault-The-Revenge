using Keru.Scripts.Engine.Master;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Keru.Scripts.Game.Menus.GameMainMenu.PowerSelection
{
    public class PowerSelectObject : MonoBehaviour
    {
        [SerializeField] private Text _name;
        [SerializeField] private Text _description;
        [SerializeField] private Image _icon;
        [SerializeField] private List<Button> _abilityChangeButtons;

        private List<AbilityData> _unlockedAbilities;
        private List<Sprite> _abilityIcons;
        private int _currentIndex;
        private AbilityCodes _currentAbility;

        public void LoadAbilityData(List<AbilityData> abilities, AbilityCodes currentCode, List<Sprite> abilityIcons)
        {
            _unlockedAbilities = abilities;
            _currentAbility = currentCode;
            _abilityIcons = abilityIcons;
            ChangeAbility(0);

            if (_unlockedAbilities.Count <= 1)
            {
                foreach (var button in _abilityChangeButtons)
                {
                    button.enabled = false;
                }
            }
        }

        public void ChangeAbility(int direction)
        {
            if (direction == 0)
            {
                var ability = _unlockedAbilities.First(x => x.Code == _currentAbility);
                _currentIndex = _unlockedAbilities.IndexOf(ability);
            }
            else
            {
                if (_currentIndex >= _unlockedAbilities.Count)
                {
                    _currentIndex = 0;
                }
                if (_currentIndex < 0)
                {
                    _currentIndex = _unlockedAbilities.Count - 1;
                }
            }

            UpdateAbilityValues(_unlockedAbilities[_currentIndex]);
        }

        private void UpdateAbilityValues(AbilityData abilityData)
        {
            _name.text = abilityData.Name.ToUpper();
            var descriptionText = abilityData.Description;
            var abilityPowers = abilityData.DataPerLevel[LevelBase.CurrentSave.UnlockedSkills.First(x => x.Code == abilityData.Code).Level - 1];

            descriptionText = descriptionText.Replace("{Power}", abilityPowers.Power.ToString());
            descriptionText = descriptionText.Replace("{Duration}", abilityPowers.Duration.ToString());
            descriptionText = descriptionText.Replace("{Cooldown}", abilityPowers.CoolDown.ToString());
            descriptionText = descriptionText.Replace("{Range}", abilityPowers.Range.ToString());

            _description.text = descriptionText.ToUpper();
            _icon.sprite = _abilityIcons[(int)abilityData.Code];

        }

        public AbilityCodes GetCurrentSkill()
        {
            return _unlockedAbilities[_currentIndex].Code;
        }
    }

}