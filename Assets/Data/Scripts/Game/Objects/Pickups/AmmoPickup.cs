using Keru.Scripts.Game.Entities.Player;
using UnityEngine;

namespace Keru.Scripts.Game.Objects.Pickups
{
    public class AmmoPickup : Pickup
    {
        [SerializeField] private float _magAmmount = 1;
        [SerializeField] private bool _appliesToBoth;
        [SerializeField] private WeaponSlot _weaponSlot = WeaponSlot.PRIMARY;

        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            var player = other.GetComponent<Player>();
            if (player != null)
            {
                var result = player.AddAmmo(_magAmmount, _appliesToBoth, _weaponSlot, _soundToPlay);
                if (result)
                {
                    Destroy(gameObject);
                }
            }
        }

        public void SetConfig(int magAmmount, bool appliesToBoth, WeaponSlot weaponSlot)
        {
            _magAmmount = magAmmount;
            _appliesToBoth = appliesToBoth;
        }
    }
}

