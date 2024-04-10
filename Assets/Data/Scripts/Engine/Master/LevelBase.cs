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
        public static LevelBase Singleton;
        public static SaveGameFile CurrentSave;
        public static MainGameData GameOptions;

        [SerializeField] private bool _isMenu;
        [SerializeField] private bool _usesMusic = true;
        [SerializeField] private float _localTimeScale = 1f;

        private AudioManager _audioManager;
        private GraphicsManager _graphicsManager;
        private JukeBox _jukeBox;
        private Fading _fading;
        private Volume _volume;

        private void Awake()
        {
            Singleton = this;
            DataLoad();
            LoadModules();

            if(_isMenu)
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

        private void DataLoad()
        {
            GameOptions = ExternalFilesManager.LoadGameData();
            CurrentSave = ExternalFilesManager.LoadSavedGame(GameOptions.SaveGameLocation);
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

            _jukeBox = GetComponent<JukeBox>();
            _jukeBox.SetUp(GameOptions.AlternateMusic);
        }
    }
}
