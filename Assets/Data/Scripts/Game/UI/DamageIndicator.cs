using UnityEngine;

namespace Keru.Scripts.Game.UI
{
    public class DamageIndicator : MonoBehaviour
    {
        private Transform _objective;

        public void SetConfig(Transform objective)
        {
            _objective = objective;
            Destroy(gameObject, 2f);
        }

        private void Update()
        {
            if(_objective != null)
            {
                var positionNormalized = new Vector3(_objective.position.x, transform.position.y, _objective.position.z);
                transform.forward = positionNormalized - transform.position;
            }
        }
    }
}
