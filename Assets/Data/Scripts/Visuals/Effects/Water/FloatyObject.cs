using UnityEngine;

namespace Keru.Scripts.Visuals.Effects.Water
{
    public class FloatyObject : MonoBehaviour
    {
        private float _depthBeforeSubmerged;
        private float _displacementAmmount;
        private float _verticalPoint;
        private Rigidbody _rigidbody;
        private float _oldRigidbodyDrag;

        public void Setup(float depthBeforeSubmerged, float displacementAmmount, float verticalPoint)
        {
            _depthBeforeSubmerged = depthBeforeSubmerged;
            _displacementAmmount = displacementAmmount;
            _verticalPoint = verticalPoint;

            _rigidbody = GetComponent<Rigidbody>();
            _oldRigidbodyDrag = _rigidbody.drag;
            _rigidbody.drag = 0.5f;
        }

        private void FixedUpdate()
        {
            if (transform.position.y < _verticalPoint)
            {
                var displacementMultiplier = Mathf.Clamp01((_verticalPoint - transform.position.y) / _depthBeforeSubmerged) * _displacementAmmount;
                _rigidbody.drag = 0.5f * (displacementMultiplier);
                _rigidbody.AddForce(new Vector3(0f, Mathf.Abs(Physics.gravity.y)) * displacementMultiplier, ForceMode.Acceleration);
            }
        }

        private void OnDestroy()
        {
            _rigidbody.drag = _oldRigidbodyDrag;
        }
    }
}