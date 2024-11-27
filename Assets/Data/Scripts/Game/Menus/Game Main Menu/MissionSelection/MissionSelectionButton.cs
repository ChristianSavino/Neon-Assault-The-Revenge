using Keru.Scripts.Engine;
using Keru.Scripts.Engine.Master;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Keru.Scripts.Game.Menus.GameMainMenu.MissionSelection
{
    public class MissionSelectionButton : MonoBehaviour
    {
        [Header("Game Objects")]
        [SerializeField] private GameObject _completedMark;
        [SerializeField] private GameObject _selectedButtonAnim;
        [SerializeField] private GameObject _descriptionObject;

        [Header("Description")]
        [SerializeField] private Text _title;
        [SerializeField] private Text _description;
        [SerializeField] private Text _completedTime;

        private LevelCode _levelCode;

        private void Start()
        {
            _levelCode = (LevelCode)Enum.Parse(typeof(LevelCode), gameObject.name);

            var levelData = LevelBase.CurrentSave.AllLevelData.Where(x => x.Code == _levelCode).FirstOrDefault();
            if (levelData == null)
            {
                gameObject.SetActive(false);
            }
            else
            {
                var mainLevelData = MasterLevelData.AllLevels.Where(x => x.Code == levelData.Code).FirstOrDefault();
                if (mainLevelData != null)
                {
                    _title.text = mainLevelData.LevelName.ToUpper();
                    _description.text = mainLevelData.Description.ToUpper();
                    _completedTime.text += levelData.CompletedTime.ToString();

                    _descriptionObject.SetActive(false);
                    _completedMark.SetActive(levelData.Completed);
                    _selectedButtonAnim.SetActive(false);
                }
               
            }
        }

        public void ToggleDescription(bool on)
        {
            _descriptionObject.SetActive(on);
        }

        public void SelectLevel()
        {
            var mainGameMenuScript = GetComponentInParent<GameMainMenuData>();
            mainGameMenuScript.SetLevelCode(_levelCode, this);
            _selectedButtonAnim.SetActive(true);
        }

        public void DeselectLevel()
        {
            _selectedButtonAnim.SetActive(false);
        }
    }
}
