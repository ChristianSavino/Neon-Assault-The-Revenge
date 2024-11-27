using Keru.Scripts.Engine.Master;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Keru.Scripts.Game.Menus.GameMainMenu.PowerSelection
{
    public class PowerSelectMainMenu : MonoBehaviour
    {
        [SerializeField] private Light _light;
        [SerializeField] private PowerSelectObject _secondary;
        [SerializeField] private PowerSelectObject _ultimate;
        [SerializeField] private List<Sprite> _abilityIcons;

        private void OnEnable()
        {
            _light.enabled = true;
        }

        private void Start()
        {
            var currentCharacter = LevelBase.CurrentSave.CurrentCharacterData;
            var allAbilities = LevelBase.CurrentSave.UnlockedSkills;
            var unlockedUltimates = MasterAbilityData.UltimateAbilities.Where(x => allAbilities.Any(g => g.Code == x.Code));
            var unlockedSecondaries = MasterAbilityData.SecondaryAbilities.Where(x => allAbilities.Any(g => g.Code == x.Code));

            _ultimate.LoadAbilityData(unlockedUltimates.ToList(), currentCharacter.UltimateSkill, _abilityIcons);
            _secondary.LoadAbilityData(unlockedSecondaries.ToList(), currentCharacter.SecondarySkill, _abilityIcons);
        }

        private void OnDisable()
        {
            _light.enabled = false;
            var currentCharacter = LevelBase.CurrentSave.CurrentCharacterData;
            currentCharacter.UltimateSkill = _ultimate.GetCurrentSkill();
            currentCharacter.SecondarySkill = _secondary.GetCurrentSkill();
        }
    }
}