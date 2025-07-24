using Keru.Scripts.Game.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Keru.Scripts.Game.Entities.Player.UI
{
    public class PassiveUIHandler : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _containers;
        [SerializeField] private PassiveTimer _example;
        
        private List<PassiveTimer> _passives = new List<PassiveTimer>();
        
        public void SetConfig()
        {

        }

        public void AddPassive(PassiveStats stats)
        {
            var passive = Instantiate(_example.gameObject).GetComponent<PassiveTimer>();
            passive.SetConfig(stats, this);
            _passives.Add(passive);
            AssignPassives();
        }

        public void PassiveDestroy()
        {
            StartCoroutine(WaitForDestroyPassive());
        }

        private void AssignPassives()
        {
            _passives = _passives.Where(x => x != null || !x.Equals(null)).ToList();
            var counter = 0;
            foreach (var passive in _passives)
            {
                var container = _containers[counter];
                passive.transform.SetParent(container.transform, false);
                passive.GetComponent<RectTransform>().SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
                counter++;
            }
        }

        private IEnumerator WaitForDestroyPassive()
        {
            yield return new WaitForEndOfFrame();
            AssignPassives();
        }
    }
}