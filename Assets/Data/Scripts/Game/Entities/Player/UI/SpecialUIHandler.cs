using Keru.Scripts.Game.Specials;
using System.Collections.Generic;
using UnityEngine;

namespace Keru.Scripts.Game.Entities.Player.UI
{
    public class SpecialUIHandler : MonoBehaviour
    {
        [SerializeField] private SpecialDataUIHandler _secondaryBlock;
        [SerializeField] private SpecialDataUIHandler _ultimateBlock;

        private Dictionary<string, KeyCode> _keys;

        public void SetConfig(Dictionary<string, KeyCode> keys)
        {
            _secondaryBlock.gameObject.SetActive(false);
            _ultimateBlock.gameObject.SetActive(false);

            _keys = keys;
        }

        public void SetConfigSpecial(Special special)
        {
            if (special.GetStats().AbilitySlot == AbilitySlot.ULTIMATE)
            {
                _ultimateBlock.gameObject.SetActive(true);
                special.SetUIHandler(_ultimateBlock, _keys["Special"]);
            }
            else
            {
                _secondaryBlock.gameObject.SetActive(true);
                special.SetUIHandler(_secondaryBlock, _keys["Second"]);
            }
        }
    }
}