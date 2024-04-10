using Keru.Scripts.Engine.FileSystem;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Keru.Scripts.Game.Menus
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _continueSubMenu;
        [SerializeField] private GameObject _newGameSubMenu;
        [SerializeField] private GameObject _optionsMenu;
        [SerializeField] private Button _continueButton;
        [SerializeField] private Text _continueButtonText;

        private void OnEnable()
        {
            BackMenu();
        }

        private void Awake()
        {
            var hasSavedGames = ExternalFilesManager.LoadAllSavedGames().Where(x => x != null).Any();

            _continueButton.interactable = hasSavedGames;
            _continueButtonText.color = hasSavedGames ? _continueButton.colors.normalColor : _continueButton.colors.disabledColor;
        }

        public void OpenContinueSubMenu()
        {
            BackMenu();
            _continueSubMenu.SetActive(true);
        }

        public void OpenNewGameSubMenu()
        {
            BackMenu();
            _newGameSubMenu.SetActive(true);
        }

        public void OpenOptionsMenu()
        {
            BackMenu();
            _optionsMenu.SetActive(true);
            gameObject.SetActive(false);
        }

        public void Quit()
        {
            Application.Quit();
        }
        
        public void BackMenu()
        {
            _continueSubMenu.SetActive(false);
            _newGameSubMenu.SetActive(false);
        }
    }
}
