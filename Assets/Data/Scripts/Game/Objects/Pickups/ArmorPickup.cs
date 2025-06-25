using Keru.Scripts.Game.Entities.Player;
using Keru.Scripts.Game.Objects.Pickups;
using UnityEngine;

public class ArmorPickup : Pickup
{
    [SerializeField] private int _armorAmount = 100;

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        var player = other.GetComponent<Player>();
        var result = player.AddArmor(_armorAmount, _soundToPlay);
        if (result)
        {
            Destroy(gameObject);
        }
    }

    public void SetConfig(int armorAmount)
    {
        _armorAmount = armorAmount;
    }
}
