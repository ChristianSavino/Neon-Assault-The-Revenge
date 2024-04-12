using Keru.Scripts.Engine.FileSystem;
using Keru.Scripts.Engine.Module;
using Keru.Scripts.Visuals.Effects;
using System.Collections;
using System.Collections.Generic;
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

            CurrentSave = _saveManager.LoadSaveGame(GameOptions.SaveGameLocation);

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
