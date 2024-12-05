using Keru.Scripts.Engine.Module;
using System.Collections;
using UnityEngine;

namespace Keru.Scripts.Engine.Debug
{
    public class DebugIntro : MonoBehaviour
    {
        [SerializeField] private GameObject _button;
        [SerializeField] private float _secondsToWait = 5f;

        private void Start()
        {
            _button.SetActive(false);

            StartCoroutine(SpawnButton());
        }

        private IEnumerator SpawnButton()
        {
            yield return new WaitForSecondsRealtime(_secondsToWait);

            _button.SetActive(true);
        }

        public void GoMainMenu()
        {
            LevelSceneManager.levelSceneManager.LoadScene(LevelCode.MainMenu);
            _button.gameObject.SetActive(false);
        }
    }
}
