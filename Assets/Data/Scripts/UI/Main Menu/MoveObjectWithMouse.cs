using UnityEngine;

namespace Keru.Scripts.Visuals.UI
{
    public class MoveObjectWithMouse : MonoBehaviour
    {
        [SerializeField] private GameObject _objectToMove;
        [SerializeField] private bool _blockYAxis;
        private Camera _mainCamera;

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        private void Update()
        {
            var previousPosition = _objectToMove.transform.position;
            var mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);

            if (_blockYAxis)
            {
                mousePosition.y = _objectToMove.transform.position.y;
            }

            var direction = mousePosition - previousPosition;
            if (direction != Vector3.zero)
            {
                _objectToMove.transform.forward = direction;
                Physics.SyncTransforms();
            }

            _objectToMove.transform.position = mousePosition;           
        }
    }
}
