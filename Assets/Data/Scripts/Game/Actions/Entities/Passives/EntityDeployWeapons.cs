using Keru.Scripts.Game.Entities;
using Keru.Scripts.Game.Entities.Player;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Keru.Scripts.Game.Actions.Entities.Passives
{
    public class EntityDeployWeapons : MonoBehaviour
    {
        [SerializeField] private bool _onlyPlayer;
        private List<GameObject> _entitiesAlreadyDeployed;

        private void Awake()
        {
            if (!_onlyPlayer)
            {
                _entitiesAlreadyDeployed = new List<GameObject>();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_onlyPlayer)
            {
                if (other.gameObject.CompareTag("Player"))
                {
                    Player.Singleton.ForceDeployWeapon();
                    Destroy(this);
                }
            }
            else
            {
                var entity = other.gameObject.GetComponent<Entity>();
                if (entity != null && _entitiesAlreadyDeployed.First(x => x == entity.gameObject) == null)
                {
                    entity.ForceDeployWeapon();
                    _entitiesAlreadyDeployed.Add(entity.gameObject);
                }
            }
        }
    }
}