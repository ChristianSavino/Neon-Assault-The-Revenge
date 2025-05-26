using UnityEngine;

namespace Keru.Scripts.Game.Actions.Camera
{
    public class CameraFollowTarget : MonoBehaviour
    {
        private Transform _target;
        private float _y;
        private float _x;
        private float _z;

        public void SetConfig(Transform target)
        {
            _target = target;
            _y = transform.position.y;
            _x = transform.localPosition.x;
            _z = transform.localPosition.z;
        }

        private void Update()
        {
            transform.position = new Vector3(_target.position.x + _x, _y, _target.position.z + _z);
        }
    }
}