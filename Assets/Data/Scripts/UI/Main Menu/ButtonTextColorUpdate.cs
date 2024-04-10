using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Keru.Scripts.Visuals.UI
{
    public class ButtonTextColorUpdate : MonoBehaviour
    {
        private Text _buttonText;
        private Button _button;
        
        void Start()
        {
            _button = GetComponent<Button>();
            _buttonText = GetComponentInChildren<Text>();

            if(!_button.interactable)
            {
                Destroy(GetComponent<EventTrigger>());
                Destroy(this);
            }
            else
            {
                _buttonText.color = _button.colors.normalColor;
            }
        }

        public void PointerEnter()
        {
            _buttonText.color =_button.colors.highlightedColor;
        }

        public void PointerClick()
        {
            _buttonText.color = _button.colors.selectedColor;
        }

        public void PointerExit()
        {
            _buttonText.color = _button.colors.normalColor;

        }
    }
}