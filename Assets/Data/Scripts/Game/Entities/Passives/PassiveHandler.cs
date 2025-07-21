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

        public virtual Passive AddPassive(Type type = null, PassiveCode? passiveCode = null)
        {
            UpdatePassives();

            var exists = CheckIfPassiveExists(type, passiveCode);
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

        protected bool CheckIfPassiveExists(Type type = null, PassiveCode? passiveCode = null)
        {
            if(passiveCode.HasValue)
            {
                return _passives.Any(p => p.GetStats().Code == passiveCode.Value);
            }

            foreach (var p in _passives)
            {
                if (p.GetType() == type)
                {
                    return true;
                }
            }

            return false;
        }
    }
}