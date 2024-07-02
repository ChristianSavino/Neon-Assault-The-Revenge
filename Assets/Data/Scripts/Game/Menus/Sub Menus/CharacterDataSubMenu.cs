using Keru.Scripts.Engine.Master;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Keru.Scripts.Game.Menus.SubMenus
{
    public class CharacterDataSubMenu : MonoBehaviour
    {
        [SerializeField] Text _characterName;
        [SerializeField] Text _secondaryText;
        [SerializeField] Text _primaryText;
        [SerializeField] Character _character;

        private void Start()
        {
            var characterData = LevelBase.CurrentSave.Characters.First(x => x.Character == _character);
            _characterName.text += $" - NIVEL {characterData.Level} ({characterData.Exp}/{characterData.ExpToLevelUp} EXP)";
            FillAbilityData(characterData.Level, characterData.Primary, _primaryText);
            FillAbilityData(characterData.Level, characterData.Secondary, _secondaryText);
        }

        private void FillAbilityData(int level, AbilityData abilityData, Text textToFill)
        {
            var power = abilityData.PowerPerLevel.Split("/")[level - 1];
            var range = abilityData.RangePerLevel.Split("/")[level - 1];
            var duration = abilityData.DurationPerLevel.Split("/")[level - 1];
            var coolDown = abilityData.CoolDownPerLevel.Split("/")[level - 1];

            var abilityText = "";

            if (int.Parse(power) > 0)
            {
               switch(abilityData.AbilityType)
                {
                    case AbilityType.DAMAGE:
                    case AbilityType.CROWDCONTROL:
                        abilityText += $"Daño: {power} |";
                        break;
                    case AbilityType.SUPPORT:
                        abilityText += $"Curación: {power} / Mejora: {power}% |";
                        break;
                }
            }
            if(int.Parse(range) > 0)
            {
                abilityText += $"Rango: {range} |";
            }
            if(int.Parse(duration) > 0)
            {
                abilityText += $"Duración: {duration} segs. |";
            }
            if(int.Parse(coolDown) > 0)
            {
                abilityText += $"Recarga: {coolDown} segs.";
            }

            textToFill.text = abilityText.ToUpper();
        }
    }

}