using Keru.Scripts.Engine;
using Keru.Scripts.Engine.Master;
using Keru.Scripts.Game.Entities.Player.UI;
using System.Collections.Generic;
using UnityEngine;

namespace Keru.Scripts.Game.Entities.Player
{
    public class Player : Entity
    {
        public static Player Singleton;
        
        [SerializeField] private int _maxLife;
        [SerializeField] private GameObject _armorModel;
        private int _armor;

        private PlayerMovement _movement;
        private PlayerWeaponHandler _weaponHandler;
        private PlayerUIHandler _uIHandler;
        private PlayerThirdPersonAnimations _animations;
        private LifeUIHandler _lifeUIHandler;

        private Dictionary<string,KeyCode> _keys;

        private bool _paused;
        [SerializeField] private bool _debug;
        [SerializeField] private int _armorOverride;

        private void Update()
        {
            ActionKeys();
        }

        private void ActionKeys()
        {
            if(_alive)
            {
                if (Input.GetKeyDown(_keys["Pause"]))
                {
                    PauseGame();
                }
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    RestartLevel();
                }
            }
        }

        public override void OnDamagedUnit(int damage, Vector3 hitpoint, GameObject origin, DamageType damageType, float damageForce)
        {
            damage = DamageResist(damage, damageType);
            damage = DamageArmor(damage, damageType == DamageType.TRUE_DAMAGE);

            if (_alive)
            {
                SetLife(-damage, false, origin);
                if (_life <= 0)
                {
                    _alive = false;
                    Die(hitpoint, damageForce);
                }
            }
        }

        public void PauseGame(bool overwrite = false)
        {
            _paused = overwrite ? false : !_paused;
            LevelBase.levelBase.PauseManager(_paused);
            _uIHandler.SetPause(_paused);
        }

        public void ConfigPlayer(MainGameData gameOptions, SaveGameFile saveGame)
        {
            var camera = Camera.main;
            Singleton = this;
            _movement = gameObject.GetComponent<PlayerMovement>();
            _weaponHandler = gameObject.GetComponent<PlayerWeaponHandler>();
            _animations = gameObject.GetComponent<PlayerThirdPersonAnimations>();

            _uIHandler = camera.GetComponentInChildren<PlayerUIHandler>();
            _lifeUIHandler = camera.GetComponentInChildren<LifeUIHandler>();

            _keys = gameOptions.Options.PlayerControls.Keys;
            _maxLife = saveGame.CurrentCharacterData.MaxHealth;
            _life = saveGame.CurrentCharacterData.CurrentHealth;
            if(_debug)
            {
                SetArmorValue(Mathf.Clamp(_armorOverride, 0, 100));
            }
            else
            {
                SetArmorValue(saveGame.CurrentCharacterData.CurrentArmor);
            }
            

            _movement.SetConfig(_animations, _uIHandler , _keys, gameOptions.Options.PlayerControls.Sensibility);
            _animations.SetConfig();

            _weaponHandler.SetConfig(_animations, saveGame, _keys);
            _uIHandler.SetConfig();
            _lifeUIHandler.SetConfig(_life, _maxLife);
        }

        private void Die(Vector3 hitpoint, float damageForce)
        {
            _movement.Die();
            _weaponHandler.Die();
            _animations.Die(hitpoint, damageForce);
            _lifeUIHandler.Die();
            _uIHandler.Die();

            LevelBase.levelBase.SetTimeScale(0.5f);
        }

        public override void ForceDeployWeapon()
        {
            _weaponHandler.DeployWeapon(null, true);
        }

        public void RestartLevel()
        {
            LevelBase.levelBase.PlayerDeathHandler();
            Destroy(this);
        }

        public int DamageResist(int damage, DamageType damageType)
        {
            damage = damageType == DamageType.EXPLOSION ? Mathf.RoundToInt(damage * 0.25f) : damage;

            if(damageType == DamageType.TRUE_DAMAGE)
            {
                return damage;
            }

            var difficulty = LevelBase.CurrentSave.Difficulty;
            switch (difficulty) {
                case Difficulty.Easy:
                    return Mathf.RoundToInt(damage * 0.25f);
                case Difficulty.Normal:
                    return Mathf.RoundToInt(damage * 0.5f);
                case Difficulty.Hard:
                    return Mathf.RoundToInt(damage * 0.75f);
                case Difficulty.KeruMustDie:
                    return damage;
            }

            // Default case, should not happen
            return damage;
        }


        public int DamageArmor(int damage, bool trueDamage = false)
        {
            var healthDamage = damage;
            if(!trueDamage)
            {
                var percentageProtected = Mathf.Clamp(_armor / 100f, 0f, 0.9f);
                healthDamage = Mathf.RoundToInt(damage * (1 - percentageProtected));
            }
            SetArmorValue(-Mathf.RoundToInt(damage * 0.5f));

            return healthDamage;
        }

        private void SetArmorValue(int armor)
        {
            _armor += armor;
            _armor = Mathf.Clamp(_armor, 0, 100);
            _armorModel.SetActive(_armor > 0);
            _lifeUIHandler.SetArmor(_armor);
        }

        public void SetLife(int life, bool isHealing = true, GameObject origin = null)
        {
            _life += life;
            _life = Mathf.Clamp(_life, 0, _maxLife);
            _lifeUIHandler.SetLife(_life, isHealing, origin.transform);
        }

        public void SetArmor(int armor)
        {
           SetArmorValue(armor);
        }
    }
}