using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Keru.Scripts.Game.Menus
{
    public class OptionsMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _graphicsSubMenu;
        [SerializeField] private GameObject _soundSubMenu;
        [SerializeField] private GameObject _gameplaySubMenu;
        [SerializeField] private GameObject _mainMenu;
        
        private void OnEnable()
        {
            BackMenu();
        }

        public void BackMenu()
        {
            _graphicsSubMenu.SetActive(false);
            _soundSubMenu.SetActive(false);
            _gameplaySubMenu.SetActive(false);
        }

        public void OpenGraphicsSubMenu()
        {
            BackMenu();
            _graphicsSubMenu.SetActive(true);
        }

        public void OpenSoundSubMenu()
        {
            BackMenu();
            _soundSubMenu.SetActive(true);
        }

        public void OpenGameplaySubMenu()
        {
            BackMenu();
            _gameplaySubMenu.SetActive(true);
        }

        public void Back()
        {
            BackMenu();
            _mainMenu.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
