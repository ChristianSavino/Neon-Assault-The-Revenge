using Keru.Scripts.Game.Entities.Player;
using UnityEngine;

namespace Keru.Scripts.Game.Objects.Pickups
{
    public class HealthPickup : Pickup
    {
        [SerializeField] private int _healthAmount = 100;

        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            var player = other.GetComponent<Player>();
            if(player != null)
            {
                var result = player.AddLife(_healthAmount, _soundToPlay);
                if (result)
                {
                    Destroy(gameObject);
                }
            }
        }

        public void SetConfig(int healthAmount)
        {
            _healthAmount = healthAmount;
        }
    }
}
