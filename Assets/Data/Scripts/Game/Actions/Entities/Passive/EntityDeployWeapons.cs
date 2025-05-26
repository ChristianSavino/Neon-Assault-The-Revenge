using Keru.Scripts.Game.Entities;
using Keru.Scripts.Game.Entities.Player;
using UnityEngine;

namespace Keru.Scripts.Game.Actions.Entities.Passive
{
    public class EntityDeployWeapons : MonoBehaviour
    {
        [SerializeField] private bool _onlyPlayer;

        private void OnTriggerEnter(Collider other)
        {
            if(_onlyPlayer)
            {
                if (other.gameObject.CompareTag("Player"))
                {
                    Player.Singleton.ForceDeployWeapon();
                }
            }
            else
            {
                var entity = other.gameObject.GetComponent<Entity>();
                if(entity != null)
                {
                    entity.ForceDeployWeapon();
                }
            }
        }
    }
}
