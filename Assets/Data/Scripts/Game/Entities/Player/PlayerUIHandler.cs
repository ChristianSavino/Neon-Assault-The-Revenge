using UnityEngine;

namespace Keru.Scripts.Game.Entities.Player
{
    public class PlayerUIHandler : MonoBehaviour
    {
        [SerializeField] private GameObject _inGameHud;
        [SerializeField] private GameObject _pauseMenu;
        [SerializeField] private GameObject _deathHud;

        public void SetConfig()
        {
            _inGameHud.SetActive(true);
            _pauseMenu.SetActive(true);
            _deathHud.SetActive(false);

            _pauseMenu.transform.localScale = Vector3.zero;
            _inGameHud.transform.localScale = Vector3.one;
        }

        public void SetPause(bool toggle)
        {
            _pauseMenu.transform.localScale = toggle ? Vector3.one : Vector3.zero;
            _inGameHud.transform.localScale = toggle ? Vector3.zero : Vector3.one;
        }

        public void Die()
        {
            _inGameHud.SetActive(false);
            _pauseMenu.SetActive(false);
            _deathHud.SetActive(true);
        }
    }
}