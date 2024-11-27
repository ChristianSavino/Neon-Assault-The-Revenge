using Keru.Scripts.Engine;
using Keru.Scripts.Engine.Master;
using Keru.Scripts.Game.Menus.GameMainMenu.MissionSelection;
using Keru.Scripts.Helpers;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace Keru.Scripts.Game.Menus.GameMainMenu
{
    public class GameMainMenuData : MonoBehaviour
    {
        private LevelCode? _missionSelected;
        private MissionSelectionButton _selectedMissionButton;
        private IEnumerable<GameMainMenuObject> _menuObjects;
        private GameObject _lastSelectedMenu;

        [SerializeField] private Text _subMenuDescription;
        [SerializeField] private GameObject _backButton;
        [SerializeField] private Text _clock;
        [SerializeField] private List<CameraPosition> _cameraPositions;

        private void Start()
        {
            _menuObjects = FindObjectsOfType(typeof(GameMainMenuObject)) as GameMainMenuObject[];
            _backButton.SetActive(false);
            ChangeCameraPosition(0);
        }

        private void Update()
        {
            _clock.text = ClockHelper.GetCurrentTime();
        }

        public void SetLevelCode(LevelCode levelCode, MissionSelectionButton selectedMissionButton)
        {
            if (_selectedMissionButton != null)
            {
                _selectedMissionButton.DeselectLevel();
            }

            _missionSelected = levelCode;
            _selectedMissionButton = selectedMissionButton;
        }

        public LevelCode? GetLevelCode()
        {
            return _missionSelected;
        }

        public void PrepareDataForNextLevel(bool hasAlternateMusic)
        {
            var saveGame = LevelBase.CurrentSave;
            saveGame.CurrentLevelCode = _missionSelected.Value;
            saveGame.AlternateMusic = hasAlternateMusic;

            LevelBase.levelBase.LoadSelectedLevel();
        }

        public void BackToMainGameMenu()
        {
            
            _lastSelectedMenu.SetActive(false);
            ToggleAllGameMenus(true);
            _backButton.SetActive(false);
            _subMenuDescription.enabled = true;
            ChangeCameraPosition(0);
        }

        public void ToggleAllGameMenus(bool on)
        {
            foreach (var menu in _menuObjects)
            {
                menu.SetEnabled(on);
            }
        }

        public void ToggleMenu(GameObject lastSelectedMenu, int cameraPosition)
        {
            _lastSelectedMenu = lastSelectedMenu;
            _lastSelectedMenu.SetActive(true);
            _backButton.SetActive(true);

            ToggleAllGameMenus(false);
            _subMenuDescription.enabled = false;
            ChangeCameraPosition(cameraPosition);
        }

        public void UpdateSubMenuDescription(string description)
        {
            _subMenuDescription.text = description.ToUpper();
        }

        private void ChangeCameraPosition(int cameraPosition)
        {
            var cameraTransform = Camera.main.transform;
            var cameraPositionData = _cameraPositions[cameraPosition];
            cameraTransform.position = cameraPositionData.position;
            cameraTransform.rotation = Quaternion.Euler(cameraPositionData.rotation);
        }
    }

    [Serializable]
    public class CameraPosition
    {
        public Vector3 position;
        public Vector3 rotation;
    }
}