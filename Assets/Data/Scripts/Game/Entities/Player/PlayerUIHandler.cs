using UnityEngine;

namespace Keru.Scripts.Game.Entities.Player
{
    public class PlayerUIHandler : MonoBehaviour
    {
        [SerializeField] private GameObject _pauseMenu;

        private void Start()
        {
            _pauseMenu.SetActive(false);
        }

        public void SetConfig()
        {

        }

        public void SetPause(bool toggle)
        {
            _pauseMenu.SetActive(toggle);
        }
    }
}