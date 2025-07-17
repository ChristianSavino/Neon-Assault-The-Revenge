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

        public virtual Passive AddPassive(Type type)
        {
            UpdatePassives();

            var exists = CheckIfPassiveExists(type);
            if (!exists)
            {
                var passive = (Passive)gameObject.AddComponent(type);
                _passives.Add(passive);
                return passive;
            }

            return null;
        }

        public virtual void UpdatePassives()
        {
            _passives = _passives.Where(x => x != null || !x.Equals(null)).ToList();
        }

        protected bool CheckIfPassiveExists(Type type)
        {
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