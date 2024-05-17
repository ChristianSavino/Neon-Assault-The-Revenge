using Keru.Scripts.Engine.FileSystem;
using Keru.Scripts.Engine.Module;
using Keru.Scripts.Visuals.Effects;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace Keru.Scripts.Engine.Master
{
    public class LevelBase : MonoBehaviour
    {
        public static LevelBase levelBase;
        public static SaveGameFile CurrentSave;
        public static MainGameData GameOptions;

        [SerializeField] private bool _isMenu;
        [SerializeField] private bool _usesMusic = true;
        [SerializeField] private float _localTimeScale = 1f;

        private AudioManager _audioManager;
        private GraphicsManager _graphicsManager;
        private LevelSceneManager _levelSceneManager;
        private SaveManager _saveManager;
        private JukeBox _jukeBox;
        private Fading _fading;
        private Volume _volume;

        private void Awake()
        {
            levelBase = this;
            GameOptions = ExternalFilesManager.LoadGameData();
           
            LoadModules();

            if(GameOptions.SaveGameLocation != -1)
            {
                CurrentSave = _saveManager.LoadSaveGame(GameOptions.SaveGameLocation);
            }
           
            if (_isMenu)
            {

            }
            else
            {

            }

            if (_usesMusic)
            {
                _jukeBox.ChangeSong(MusicTracks.Ambience, false);
            }
        }


        private void LoadModules()
        {
            _fading = GetComponent<Fading>();
            _volume = GetComponentInChildren<Volume>();
            
            var managerGameObject = transform.Find("Modules").gameObject;

            _audioManager = managerGameObject.GetComponent<AudioManager>();
            _audioManager.SetUp();

            _graphicsManager = managerGameObject.GetComponent<GraphicsManager>();
            _graphicsManager.SetUp(_volume, _fading, _isMenu);

            _levelSceneManager = managerGameObject.GetComponent<LevelSceneManager>();
            _levelSceneManager.SetUp();

            _saveManager = managerGameObject.GetComponent<SaveManager>();
            _saveManager.SetUp();

            _jukeBox = GetComponent<JukeBox>();
            _jukeBox.SetUp(GameOptions.AlternateMusic);   
        }

        public SaveGameFile CreateNewSaveGame(int saveGameSlot)
        {
            CurrentSave = SaveManager.saveManager.CreateNewSaveGame(saveGameSlot);
            GameOptions.SaveGameLocation = saveGameSlot;
            ExternalFilesManager.UpdateGameData(GameOptions);

            return CurrentSave;
        }

        public void LoadGame(int saveGameLocation)
        {
            GameOptions.SaveGameLocation = saveGameLocation;
            CurrentSave = SaveManager.saveManager.LoadSaveGame(saveGameLocation);
            ExternalFilesManager.UpdateGameData(GameOptions);
            
            _levelSceneManager.LoadScene(CurrentSave.CurrentLevelCode);
        }

        public void CompleteLevel()
        {
            var currentLevelData = CurrentSave.AllLevelData[(int)CurrentSave.CurrentLevelCode];
            currentLevelData.Completed = true;

            if ((int)currentLevelData.Unlocks < CurrentSave.AllLevelData.Count)
            {
                var levelToUnlock = CurrentSave.AllLevelData[(int)currentLevelData.Unlocks];
                levelToUnlock.Unlocked = true;
            }

            CurrentSave.CurrentLevelCode = currentLevelData.NextLevel;
            _saveManager.SaveGame(CurrentSave);

            _levelSceneManager.LoadScene(currentLevelData.NextLevel);
        }

        public void SetTimeScale(float timeScale)
        {
            Time.timeScale = timeScale;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;
            if (timeScale == 0)
            {
                Time.fixedDeltaTime = 0.02f;
            }

            _audioManager.SetPitch(timeScale);
        }
    }
}
