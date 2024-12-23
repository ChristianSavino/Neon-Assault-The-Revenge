using Keru.Scripts.Engine.FileSystem;
using Keru.Scripts.Engine.Module;
using Keru.Scripts.Game.Entities.Player;
using Keru.Scripts.Visuals.Effects;
using System;
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
        [SerializeField] private bool _useMainVolumetricLights;
        [SerializeField] private bool _useExtraVolumetricLights;

        private AudioManager _audioManager;
        private GraphicsManager _graphicsManager;
        private LevelSceneManager _levelSceneManager;
        private SaveManager _saveManager;
        private JukeBox _jukeBox;
        private Fading _fading;
        private Volume _volume;

        private DateTime _startTime;
        private Player _player;

        private void Awake()
        {
            levelBase = this;
            GameOptions = ExternalFilesManager.LoadGameData();
           
            LoadModules();
           
            if (!_isMenu)
            {
                var player = GameObject.Find("Player");
                _player = player.GetComponent<Player>();
                _player.ConfigPlayer(GameOptions, CurrentSave);
            }

            if (_usesMusic)
            {
                _jukeBox.ChangeSong(MusicTracks.Ambience, false);
            }

            _startTime = DateTime.Now;
        }


        private void LoadModules()
        {
            _fading = GetComponent<Fading>();
            _volume = GetComponentInChildren<Volume>();
            
            var managerGameObject = transform.Find("Modules").gameObject;

            _audioManager = managerGameObject.GetComponent<AudioManager>();
            _audioManager.SetUp();

            _graphicsManager = managerGameObject.GetComponent<GraphicsManager>();
            _graphicsManager.SetUp(_volume, _fading, _isMenu, _useMainVolumetricLights, _useExtraVolumetricLights);

            _levelSceneManager = managerGameObject.GetComponent<LevelSceneManager>();
            _levelSceneManager.SetUp();

            _saveManager = managerGameObject.GetComponent<SaveManager>();
            _saveManager.SetUp();

            if (GameOptions.SaveGameLocation != -1)
            {
                CurrentSave = _saveManager.LoadSaveGame(GameOptions.SaveGameLocation);
            }

            _jukeBox = GetComponent<JukeBox>();
            if (CurrentSave != null)
            {
                _jukeBox.SetUp(CurrentSave.AlternateMusic);
            }
            else
            {
                _jukeBox.SetUp(false);
            }
        }

        public SaveGameFile CreateNewSaveGame(int saveGameSlot, int difficulty)
        {
            CurrentSave = SaveManager.saveManager.CreateNewSaveGame(saveGameSlot, difficulty);
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
            var currentSaveLevelData = CurrentSave.AllLevelData.FirstOrDefault(x => x.Code == CurrentSave.CurrentLevelCode);
            if(currentSaveLevelData != null)
            {
                currentSaveLevelData.Completed = true;
                currentSaveLevelData.CompletedTime = DateTime.Now - _startTime;
            }      

            var mainLevelData = MasterLevelData.AllLevels.First(x => x.Code == CurrentSave.CurrentLevelCode);

            foreach (var unlockedLevelCode in mainLevelData.Unlocks)
            {
                var levelToUnlock = CurrentSave.AllLevelData.Find(x => x.Code == unlockedLevelCode);
                if(levelToUnlock == null)
                {
                    var levelToAdd = MasterLevelData.AllLevels.First(x => x.Code == unlockedLevelCode);
                    CurrentSave.AllLevelData.Add(new LevelSaveData()
                    {
                        Code = unlockedLevelCode,
                        Completed = false
                    });
                }
            }

            CurrentSave.CurrentLevelCode = mainLevelData.NextLevel;
            _saveManager.SaveGame(CurrentSave);

            _levelSceneManager.LoadScene(mainLevelData.NextLevel);
        }

        public void LoadSelectedLevel()
        {
            _saveManager.SaveGame(CurrentSave);
            _levelSceneManager.LoadScene(CurrentSave.CurrentLevelCode);
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

        public void PauseManager(bool paused)
        {
            if (paused)
            {
                SetTimeScale(0);
                _jukeBox.Pause(true);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                SetTimeScale(_localTimeScale);
                _jukeBox.Pause(false);
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}
