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
        [SerializeField] private GameObject _effectsGameObject;
        private int _armor;

        private PlayerMovement _movement;
        private PlayerWeaponHandler _weaponHandler;
        private PlayerUIHandler _uIHandler;
        private PlayerThirdPersonAnimations _animations;
        private LifeUIHandler _lifeUIHandler;
        private PlayerSpecialHandler _specialHandler;
        private PlayerEffectsHandler _effectsHandler;

        private Dictionary<string,KeyCode> _keys;

        private bool _paused;
        [SerializeField] private bool _debug;
        [SerializeField] private int _armorOverride;

        [Header("Armor")]
        [SerializeField] private GameObject _armorModel;
        [SerializeField] private GameObject _armorBrokenEffects;
        [SerializeField] private AudioClip _armorBrokenSoundEffect;

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
            _specialHandler = gameObject.GetComponent<PlayerSpecialHandler>();
            _animations = gameObject.GetComponent<PlayerThirdPersonAnimations>();

            _uIHandler = camera.GetComponentInChildren<PlayerUIHandler>();
            _lifeUIHandler = camera.GetComponentInChildren<LifeUIHandler>();
            var weaponUiHandler = camera.GetComponentInChildren<WeaponUIHandler>();
            var specialUiHandler = camera.GetComponentInChildren<SpecialUIHandler>();
            _effectsHandler = _effectsGameObject.GetComponent<PlayerEffectsHandler>();

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
            

            _movement.SetConfig(this, _animations, _uIHandler , _keys, gameOptions.Options.PlayerControls.Sensibility);
            _animations.SetConfig();

            _weaponHandler.SetConfig(_animations, saveGame, _keys, weaponUiHandler, gameOptions.Options.PlayerControls.AutoReload);
            _uIHandler.SetConfig();
            _lifeUIHandler.SetConfig(_life, _maxLife);
            _specialHandler.SetConfig(saveGame, _keys, _movement, _weaponHandler, _animations, specialUiHandler);
            _effectsHandler.SetConfig();

            _passiveHandler.SetUp(_animations.GetModelObject());
            var playerPassiveHandler = _passiveHandler as PlayerPassiveHandler;
            playerPassiveHandler.SetUpPlayer(this, _weaponHandler, _movement, _animations, _specialHandler);
        }

        private void Die(Vector3 hitpoint, float damageForce)
        {
            _movement.Die();
            _weaponHandler.Die();
            _animations.Die(hitpoint, damageForce);
            _lifeUIHandler.Die();
            _uIHandler.Die();
            _specialHandler.Die();

            LevelBase.levelBase.Die();
        }

        public override void ForceDeployWeapon()
        {
            _weaponHandler.DeployWeapon(null, true);
        }

        private void RestartLevel()
        {
            LevelBase.levelBase.PlayerDeathHandler();
            Destroy(this);
        }

        private int DamageResist(int damage, DamageType damageType)
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

        private int DamageArmor(int damage, bool trueDamage = false)
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
            var hasArmor = _armor > 0;
            _armor += armor;
            _armor = Mathf.Clamp(_armor, 0, 100);
            if(hasArmor && _armor == 0)
            {
                AddEffect(_armorBrokenSoundEffect, _armorBrokenEffects);
            }
            _armorModel.SetActive(_armor > 0);
            _lifeUIHandler.SetArmor(_armor);
        }

        private void SetLife(int life, bool isHealing = true, GameObject origin = null)
        {       
            _life += life;
            _life = Mathf.Clamp(_life, 0, _maxLife);
            _lifeUIHandler.SetLife(_life, isHealing, origin?.transform);
        }

        public bool AddLife(int life, AudioClip clip = null)
        {
            if(_life >= _maxLife)
            {
                return false;
            }

            if(clip != null)
            {
                _effectsHandler.CreateEffect(clip);
            }

            SetLife(life, true);
            return true;
        }

        public void SetArmor(int armor)
        {
           SetArmorValue(armor);
        }

        public bool AddArmor(int armor, AudioClip clip = null)
        {
            if (_armor >= 100)
            {
                return false;
            }
            
            if (clip != null)
            {
                _effectsHandler.CreateEffect(clip);
            }
            
            SetArmorValue(armor);
            return true;
        }

        public bool AddAmmo(float magAmmount, bool appliesForBoth, WeaponSlot weaponSlot, AudioClip clip = null)
        {
            if (_weaponHandler.RefillAmmo(magAmmount, appliesForBoth, weaponSlot))
            {
                if (clip != null)
                {
                    _effectsHandler.CreateEffect(clip);
                }
                return true;
            }

            return false;
        }

        public void AddEffect(AudioClip soundEffect = null, GameObject effect = null, float? overrideDuration = null)
        {
            _effectsHandler.CreateEffect(soundEffect, effect, overrideDuration);
        }
    }
}