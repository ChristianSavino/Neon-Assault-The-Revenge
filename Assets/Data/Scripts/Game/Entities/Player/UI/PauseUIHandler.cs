using Keru.Scripts.Engine;
using Keru.Scripts.Engine.Master;
using Keru.Scripts.Engine.Module;
using Keru.Scripts.Game.Menus;
using UnityEngine;
using UnityEngine.UI;

namespace Keru.Scripts.Game.Entities.Player.UI
{
    public class PauseUIHandler : MonoBehaviour
    {
        [SerializeField] private MainMenu _mainMenu;
        [SerializeField] private OptionsMenu _optionsMenu;
        [SerializeField] private GameObject _question;
        [SerializeField] private Text _questionText;

        private bool _isGameMenu;

        private void OnEnable()
        {
            _mainMenu.gameObject.SetActive(true);
            _mainMenu.BackMenu();

            _optionsMenu.BackMenu();
            _optionsMenu.gameObject.SetActive(false);
        }

        public void DePause()
        {
            PlayerBase.Singleton.PauseGame(true);
        }

        public void ReloadCheckpoint()
        {

        }

        public void GameMenu()
        {
            _question.SetActive(true); 
            _isGameMenu = true;
            _questionText.text = "Desea volver al menú de misión?".ToUpper();
        }

        public void MainMenu()
        {
            _question.SetActive(true);
            _isGameMenu = false;
            _questionText.text = "Desea volver al menú principal?".ToUpper();
        }

        public void Yes()
        {
            if (_isGameMenu)
            {
                var levelBase = LevelBase.levelBase;
                LevelBase.CurrentSave.CurrentLevelCode = LevelCode.GameMainMenu;
                levelBase.LoadSelectedLevel();
            }
            else
            {
                LevelSceneManager.levelSceneManager.LoadScene(LevelCode.MainMenu);
            }
        }

        public void No()
        {
            _question.SetActive(false);
        }
    }
}
