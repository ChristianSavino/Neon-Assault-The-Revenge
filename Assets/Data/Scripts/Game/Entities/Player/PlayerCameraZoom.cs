using UnityEngine;

namespace Keru.Scripts.Game.Entities.Player
{
    public class PlayerCameraZoom : MonoBehaviour
    {
        [SerializeField] private float _zoomSpeed = 8f;
        [SerializeField] private float _minZoom = 5f;
        [SerializeField] private float _maxZoom = 20f;
        
        private float _currentZoom = 15f;
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
            _camera.transform.LookAt(transform);
            UpdateCameraPosition();
        }

        private void Update()
        {
            CameraZoom();
        }

        private void CameraZoom()
        {
            var scroll = Input.GetAxis("Mouse ScrollWheel");
            if (Mathf.Abs(scroll) > 0.01f)
            {
                _currentZoom -= scroll * _zoomSpeed;
                _currentZoom = Mathf.Clamp(_currentZoom, _minZoom, _maxZoom);
                UpdateCameraPosition();
            }
        }

        private void UpdateCameraPosition()
        {
            Vector3 direction = _camera.transform.forward;
            _camera.transform.position = transform.position - direction * _currentZoom;
            _camera.transform.LookAt(transform.position);
        }
    }
}
