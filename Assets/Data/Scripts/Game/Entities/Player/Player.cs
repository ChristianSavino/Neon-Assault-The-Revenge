using Keru.Scripts.Engine.Master;
using System.Collections.Generic;
using UnityEngine;

namespace Keru.Scripts.Game.Entities.Player
{
    public class Player : Entity
    {
        public static Player Singleton;
        
        [SerializeField] private int _maxLife;

        private PlayerMovement _movement;
        private PlayerWeaponHandler _weaponHandler;
        private PlayerUIHandler _uIHandler;
        private PlayerThirdPersonAnimations _animations;

        private Dictionary<string,KeyCode> _keys;

        private bool _paused;

        private void Update()
        {
            ActionKeys();
        }

        private void ActionKeys()
        {
            if (Input.GetKeyDown(_keys["Pause"]))
            {
                PauseGame();
            }
        }

        public override void OnDamagedUnit(int damage, Vector3 hitpoint, GameObject origin, int damageType, float damageForce)
        {
        }

        public void PauseGame(bool overwrite = false)
        {
            _paused = overwrite ? false : !_paused;
            LevelBase.levelBase.PauseManager(_paused);
            _uIHandler.SetPause(_paused);
        }

        public void ConfigPlayer(MainGameData gameOptions, SaveGameFile saveGame)
        {
            Singleton = this;
            _movement = gameObject.GetComponent<PlayerMovement>();
            _weaponHandler = gameObject.GetComponent<PlayerWeaponHandler>();
            _uIHandler = gameObject.GetComponentInChildren<PlayerUIHandler>();
            _animations = gameObject.GetComponent<PlayerThirdPersonAnimations>();

            _keys = gameOptions.Options.PlayerControls.Keys;
            
            _maxLife = saveGame.CurrentCharacterData.MaxHealth;
            _life = saveGame.CurrentCharacterData.CurrentHealth;

            _movement.SetConfig(_animations, _uIHandler , _keys, gameOptions.Options.PlayerControls.Sensibility);
        }
    }
}