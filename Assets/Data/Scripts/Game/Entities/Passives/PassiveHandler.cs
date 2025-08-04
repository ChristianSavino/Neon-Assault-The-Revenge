using Keru.Scripts.Game.ScriptableObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Keru.Scripts.Game.Entities.Passives
{
    public class PassiveHandler : MonoBehaviour
    {
        protected GameObject _model;
        protected List<Passive> _passives;

        public virtual void SetUp(GameObject model)
        {
            _model = model;
            _passives = new List<Passive>();
        }

        public GameObject GetModel()
        {
            return _model;
        }

        public virtual Passive AddPassive(PassiveStats stats, int power, Entity owner, Type type = null, PassiveCode? passiveCode = null)
        {
            UpdatePassives();

            var exists = CheckIfPassiveExists(passiveCode);
            if (!exists)
            {
                Passive passive;
               
                if(type != null)
                {
                    passive = (Passive)gameObject.AddComponent(type);
                }
                else
                {
                    passive = gameObject.AddComponent<Passive>();
                }
                
                _passives.Add(passive);
                passive.SetUp(stats, power, owner);
                return passive;
            }

            return null;
        }

        public virtual void UpdatePassives()
        {
            _passives = _passives.Where(x => x != null || !x.Equals(null)).ToList();
        }

        public virtual void DestroyPassive(Passive passive)
        {
            _passives.Remove(passive);
            UpdatePassives();
        }

        public virtual void Die()
        {
            foreach (Passive passive in _passives)
            {
                passive.Die();
            }
        }

        protected bool CheckIfPassiveExists(PassiveCode? passiveCode = null)
        {
            if(passiveCode.HasValue)
            {
                return _passives.Any(p => p.GetStats().Code == passiveCode.Value);
            }

            return false;
        }
    }
}