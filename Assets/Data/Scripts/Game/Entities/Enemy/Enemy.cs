using Keru.Scripts.Engine;
using Keru.Scripts.Engine.Master;
using Keru.Scripts.Game.Entities.Humanoid;
using Keru.Scripts.Game.Entities.Player;
using UnityEngine;

namespace Keru.Scripts.Game.Entities.Enemy
{
    public class Enemy : NPC
    { 
        private ThirdPersonAnimations _animations;
        private GameObject _model;

        private void Start()
        {
            SetConfig();
        }

        public override void SetConfig()
        {
            base.SetConfig();

            _life = _maxLife;
            
            _animations = GetComponent<ThirdPersonAnimations>();
            _animations.SetConfig();
            _model = _animations.GetModelObject();

            _collider = GetComponent<CapsuleCollider>();
            _passiveHandler.SetUp(_animations.GetModelObject());


            switch (LevelBase.CurrentSave.Difficulty)
            {
                case Difficulty.Easy:
                    _decisionInterval = 1f;
                    _acuracyMultiplier = 1.5f;
                    break;
                case Difficulty.Normal:
                    _decisionInterval = 0.75f;
                    _acuracyMultiplier = 1f;
                    break;
                case Difficulty.Hard:
                    _decisionInterval = 0.5f;
                    _acuracyMultiplier = 0.75f;
                    break;
                case Difficulty.KeruMustDie:
                    _decisionInterval = 0.2f;
                    _acuracyMultiplier = 0.5f;
                    break;
            }

            _target = PlayerBase.Singleton.transform;
        }

        protected override void Update()
        {
            base.Update();

            UpdateModelDirection(_agent.velocity.normalized);
        }

        private void UpdateModelDirection(Vector3 direction)
        {
            if (direction.sqrMagnitude > 0.01f)
            {
                var targetRotation = Quaternion.LookRotation(direction, Vector3.up);
                _model.transform.rotation = Quaternion.Lerp(_model.transform.rotation, targetRotation, Time.deltaTime * 10f);

                var localDir = _model.transform.InverseTransformDirection(direction);

                _animations.SetParameter("X", localDir.x);
                _animations.SetParameter("Y", localDir.z);
            }
            else
            {
                _animations.SetParameter("X", 0f);
                _animations.SetParameter("Y", 0f);
            }
        }

        protected override void Think()
        {
            var newAction = GetNewAction();
            if(_currentAction != newAction)
            {
                _currentAction = newAction;
                ExecuteNewAction(_currentAction);
            }

        }

        private AiActions GetNewAction()
        {           
            return AiActions.PATROL; // Placeholder for actual AI logic
        }

        private void ExecuteNewAction(AiActions newAction)
        {
            switch (newAction)
            {
                case AiActions.PATROL:
                    Patrol();
                    break;
                case AiActions.ATTACK:
                    //Attack();
                    break;
                case AiActions.RELOAD:
                    //Reload();
                    break;
                case AiActions.TAKE_COVER:
                    //SeekCover();
                    break;
                case AiActions.CHASE_TARGET:
                //ChasePlayer();
                case AiActions.DIE:
                    _agent.isStopped = true;
                    break;
            }
        }

        private void Patrol()
        {
            if(_patrolPoints != null)
            {
                _agent.SetDestination(_patrolPoints.GetCurrentWaypoint());
            }
        }

        public override void OnDamagedUnit(int damage, Vector3 hitpoint, GameObject origin, DamageType damageType, float damageForce)
        {
            base.OnDamagedUnit(damage, hitpoint, origin, damageType, damageForce);

            if (_life <= 0)
            {
                if (_alive)
                {
                    Die(hitpoint, damageForce);
                    ApplyDeathEffect(damageType, damageForce, hitpoint);
                }
                _animations.ApplyForceToClosestCollider(hitpoint, damageForce);
                ApplyDeathEffect(damageType, damageForce, hitpoint);
            }
            else
            {
                CreateDamageParticle(hitpoint, _alive, damageType);
            }
        }

        private void Die(Vector3 hitpoint, float damageForce)
        {
            _alive = false;
            _collider.enabled = false;
            _animations.Die(hitpoint, damageForce);
            _passiveHandler.Die();
            
            if(_patrolPoints != null)
            {
                _patrolPoints.Die();
            }
            
            ExecuteNewAction(AiActions.DIE);
            Destroy(gameObject, 60f);
        }
    }
}
