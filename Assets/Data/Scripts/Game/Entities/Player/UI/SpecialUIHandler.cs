using Keru.Scripts.Game.Specials;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Keru.Scripts.Game.Entities.Player.UI
{
    public class SpecialUIHandler : MonoBehaviour
    {
        [SerializeField] private SpecialDataUIHandler _secondaryBlock;
        [SerializeField] private SpecialDataUIHandler _ultimateBlock;

        private Dictionary<string, KeyCode> _keys;
        private Volume _volume;

        public void SetConfig(Dictionary<string, KeyCode> keys)
        {
            _secondaryBlock.gameObject.SetActive(false);
            _ultimateBlock.gameObject.SetActive(false);
            _volume = gameObject.GetComponent<Volume>();

            _keys = keys;
        }

        public void SetConfigSpecial(Special special)
        {
            if (special.GetStats().AbilitySlot == AbilitySlot.ULTIMATE)
            {
                _ultimateBlock.gameObject.SetActive(true);
                special.SetUIHandler(this, _ultimateBlock, _keys["Special"]);
            }
            else
            {
                _secondaryBlock.gameObject.SetActive(true);
                special.SetUIHandler(this, _secondaryBlock, _keys["Second"]);
            }
        }

        public void SetVolumeProfile(VolumeProfile volumeProfile = null, float weight = 1f)
        {
            if (volumeProfile != null)
            {
                _volume.profile = volumeProfile;
            }
            
            _volume.weight = weight;
        }

        public Volume GetVolume()
        {
            return _volume;
        }
    }
}