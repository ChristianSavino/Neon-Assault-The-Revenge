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

        private Dictionary<string, KeyCode> _keys;

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
            if (_alive)
            {
                if (Input.GetKeyDown(_keys["Pause"]))
                {
                    PauseGame();
                }
            }
            else if (Input.GetKeyDown(KeyCode.R))
            {
                RestartLevel();
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

            CreateDamageParticle(hitpoint, _alive, damageType);
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

            _movement = GetComponent<PlayerMovement>();
            _weaponHandler = GetComponent<PlayerWeaponHandler>();
            _specialHandler = GetComponent<PlayerSpecialHandler>();
            _animations = GetComponent<PlayerThirdPersonAnimations>();
            _effectsHandler = _effectsGameObject.GetComponent<PlayerEffectsHandler>();

            _uIHandler = camera.GetComponentInChildren<PlayerUIHandler>();
            _lifeUIHandler = camera.GetComponentInChildren<LifeUIHandler>();
            var weaponUiHandler = camera.GetComponentInChildren<WeaponUIHandler>();
            var specialUiHandler = camera.GetComponentInChildren<SpecialUIHandler>();
            var passiveUiHandler = camera.GetComponentInChildren<PassiveUIHandler>();

            _keys = gameOptions.Options.PlayerControls.Keys;
            _maxLife = saveGame.CurrentCharacterData.MaxHealth;
            _life = saveGame.CurrentCharacterData.CurrentHealth;

            SetArmorValue(_debug
                ? Mathf.Clamp(_armorOverride, 0, 100)
                : saveGame.CurrentCharacterData.CurrentArmor);

            _movement.SetConfig(this, _animations, _uIHandler, _keys, gameOptions.Options.PlayerControls.Sensibility);
            _animations.SetConfig();
            _weaponHandler.SetConfig(_animations, saveGame, _keys, weaponUiHandler, gameOptions.Options.PlayerControls.AutoReload);
            _uIHandler.SetConfig();
            _lifeUIHandler.SetConfig(_life, _maxLife);
            _specialHandler.SetConfig(saveGame, _keys, _movement, _weaponHandler, _animations, specialUiHandler);
            _effectsHandler.SetConfig();

            _passiveHandler.SetUp(_animations.GetModelObject());
            if (_passiveHandler is PlayerPassiveHandler playerPassiveHandler)
            {
                playerPassiveHandler.SetUpPlayer(this, _weaponHandler, _movement, _animations, _specialHandler, passiveUiHandler);
            }

            _collider = GetComponent<CapsuleCollider>();
        }

        private void Die(Vector3 hitpoint, float damageForce)
        {
            _movement.Die();
            _weaponHandler.Die();
            _animations.Die(hitpoint, damageForce);
            _lifeUIHandler.Die();
            _uIHandler.Die();
            _specialHandler.Die();
            _collider.enabled = false;

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
            if (damageType == DamageType.EXPLOSION)
            {
                damage = Mathf.RoundToInt(damage * 0.25f);
            }   

            if (damageType == DamageType.TRUE_DAMAGE)
            {
                return damage;
            }
                
            return LevelBase.CurrentSave.Difficulty switch
            {
                Difficulty.Easy => Mathf.RoundToInt(damage * 0.25f),
                Difficulty.Normal => Mathf.RoundToInt(damage * 0.5f),
                Difficulty.Hard => Mathf.RoundToInt(damage * 0.75f),
                _ => damage
            };
        }

        private int DamageArmor(int damage, bool trueDamage = false)
        {
            if (trueDamage)
            {
                SetArmorValue(-Mathf.RoundToInt(damage * 0.5f));
                return damage;
            }

            var percentageProtected = Mathf.Clamp(_armor / 100f, 0f, 0.9f);
            var healthDamage = Mathf.RoundToInt(damage * (1 - percentageProtected));
            SetArmorValue(-Mathf.RoundToInt(damage * 0.5f));
            return healthDamage;
        }

        private void SetArmorValue(int armor)
        {
            var hadArmor = _armor > 0;
            _armor = Mathf.Clamp(_armor + armor, 0, 100);

            if (hadArmor && _armor == 0)
            {
                AddEffect(_armorBrokenSoundEffect, _armorBrokenEffects);
            }

            _armorModel.SetActive(_armor > 0);
            _lifeUIHandler.SetArmor(_armor);
        }

        private void SetLife(int life, bool isHealing = true, GameObject origin = null)
        {
            _life = Mathf.Clamp(_life + life, 0, _maxLife);
            _lifeUIHandler.SetLife(_life, isHealing, origin?.transform);
        }

        public bool AddLife(int life, AudioClip clip = null)
        {
            if (_life >= _maxLife)
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