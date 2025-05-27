using UnityEngine;

namespace Keru.Scripts.Game.Entities.Player
{
    public class PlayerUIHandler : MonoBehaviour
    {
        [SerializeField] private GameObject _inGameHud;
        [SerializeField] private GameObject _pauseMenu;
        [SerializeField] private GameObject _deathHud;

        private void Start()
        {
            _pauseMenu.SetActive(false);
            _inGameHud.SetActive(true);
            _deathHud.SetActive(false);
        }

        public void SetConfig()
        {

        }

        public void SetPause(bool toggle)
        {
            _pauseMenu.SetActive(toggle);
            _inGameHud.SetActive(!toggle);
        }

        public void Die()
        {
            _inGameHud.SetActive(false);
            _pauseMenu.SetActive(false);
            _deathHud.SetActive(true);
        }
    }
}