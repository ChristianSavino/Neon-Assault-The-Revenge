using UnityEngine;

namespace Keru.Scripts.Game.Menus.GameMainMenu
{
    [RequireComponent(typeof(Outline))]
    public class GameMainMenuObject : MonoBehaviour
    {
        private Outline _outline;
        private Color _originalColor;
        private float _originalWidth;
        private GameMainMenuData _gameMainMenuData;
        private bool _enabled = true;

        [Header("Outline Options")]
        [SerializeField] private Color _selectedColor;
        [SerializeField] private float _outlineWidth;
        [SerializeField] private string _menuTitle;
        [SerializeField] private GameObject _menuToToggle;
        [SerializeField] private int _cameraPosition;

        private void Start()
        {
            _outline = GetComponent<Outline>();
            _originalColor = _outline.OutlineColor;
            _originalWidth = _outline.OutlineWidth;
            _outline.enabled = true;
            _gameMainMenuData = Camera.main.GetComponentInParent<GameMainMenuData>();
        }

        private void OnMouseDown()
        {
            if(_enabled)
            {
                _gameMainMenuData.ToggleMenu(_menuToToggle, _cameraPosition);
            }
        }

        private void OnMouseEnter()
        {
            if(_enabled)
            {
                _outline.OutlineColor = _selectedColor;
                _outline.OutlineWidth = _outlineWidth;
                _gameMainMenuData.UpdateSubMenuDescription(_menuTitle);
            }
        }

        private void OnMouseExit()
        {
            _outline.OutlineColor = _originalColor;
            _outline.OutlineWidth = _originalWidth;
            _gameMainMenuData.UpdateSubMenuDescription(string.Empty);
        }

        public void SetEnabled(bool toggle)
        {
            _enabled = toggle;
        }
    }
}