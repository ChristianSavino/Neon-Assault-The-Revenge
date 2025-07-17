using Keru.Scripts.Game.Entities;
using Keru.Scripts.Game.Entities.Passives;
using Keru.Scripts.Game.ScriptableObjects;
using UnityEngine;

namespace Keru.Scripts.Game.Actions.Entities.Passives
{
    public class ApplyFire : Action
    {
        [SerializeField] private PassiveStats _firePassive;
        [SerializeField] private int _damagePerSecond;
        [SerializeField] private Color _fireColor;

        public override void SetUp(int paramater, float floatParameter, string stringParameter, GameObject gameObjectParameter, bool boolParameter)
        {
            _damagePerSecond = paramater;
            _fireColor = gameObjectParameter.GetComponent<Renderer>().material.color;
        }

        public override void Execute(GameObject target = null)
        {
            if (target != null)
            {
                var entity = target.GetComponent<Entity>();
                if (entity != null)
                {
                    var firePassive = entity.UpdatePassiveValues(typeof(FireDotPassive)) as FireDotPassive;
                    if(firePassive != null)
                    {
                        firePassive.SetFireColor(_fireColor);
                        firePassive.SetUp(_firePassive, _damagePerSecond, entity);
                        firePassive.ExecutePassive();
                    }     
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            Execute(other.gameObject);
        }
    }
}