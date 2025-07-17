using Keru.Scripts.Game.Entities.Passives;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Keru.Scripts.Game.Entities
{
    public class Entity : MonoBehaviour
    {
        [SerializeField] protected int _life;
        [SerializeField] protected bool _alive = true;
        [SerializeField] protected PassiveHandler _passiveHandler;

        protected void Start()
        {

        }

        public virtual void OnDamagedUnit(int damage, Vector3 hitpoint, GameObject origin, DamageType damageType, float damageForce)
        {
            _life -= damage;
        }

        public virtual void ForceDeployWeapon()
        {

        }

        public virtual Passive UpdatePassiveValues(Type type = null)
        {
            if(type != null)
            {
                return _passiveHandler.AddPassive(type);
            }
            else
            {
                _passiveHandler.UpdatePassives();
                return null;
            }
        }
    }
}
