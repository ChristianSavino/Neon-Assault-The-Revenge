using Keru.Scripts.Engine;
using Keru.Scripts.Engine.Master;
using Keru.Scripts.Engine.Module;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Keru.Scripts.Game.Menus
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private GameObject _continueButton;
        [SerializeField] private GameObject _loadingText;

        [SerializeField] private Text _missionName;
        [SerializeField] private Text _missionDescription;

        private bool _changeLevel;
        private LevelSceneManager _levelSceneManager;
        private LevelCode _currentLevel;
        
        private void Start()
        {            
            _currentLevel = LevelBase.CurrentSave.CurrentLevelCode;

            ToggleButton(false);
            LoadMissionData();

            _levelSceneManager = LevelSceneManager.levelSceneManager;
            _levelSceneManager.LoadByLoadScreen(_currentLevel);
            StartCoroutine(CheckIfSceneHasLoaded());
        }

        private void ToggleButton(bool toggle)
        {
            _continueButton.SetActive(toggle);
            _loadingText.SetActive(!toggle);
        }

        private void LoadMissionData()
        {
            var levelData = MasterLevelData.AllLevels.First(x => x.Code == LevelBase.CurrentSave.CurrentLevelCode);
            _missionName.text = levelData.LevelName.ToUpper();
            _missionDescription.text = levelData.Description.ToUpper();
        }

        public void ContinueButton()
        {
            StartCoroutine(FinishLoadingScene());
            _continueButton.gameObject.SetActive(false);
        }

        private IEnumerator FinishLoadingScene()
        {
            GraphicsManager.graphicsManager.FadeCamera(1);

            yield return new WaitForSecondsRealtime(2);

            _levelSceneManager.ToggleSceneLoad();
        }

        private IEnumerator CheckIfSceneHasLoaded()
        {
            while(!_levelSceneManager.HasSceneLoaded())
            {
                yield return new WaitForEndOfFrame();
            }

            ToggleButton(true);
        }
    }
}
