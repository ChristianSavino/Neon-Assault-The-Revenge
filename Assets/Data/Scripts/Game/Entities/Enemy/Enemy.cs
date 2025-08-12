using Keru.Scripts.Engine;
using Keru.Scripts.Engine.Master;
using Keru.Scripts.Game.Entities.Player;
using UnityEngine;

namespace Keru.Scripts.Game.Entities.Enemy
{
    public class Enemy : NPC
    {
        private GameObject _model;

        private bool _isShooting = false;
        private int _maxChaseTicks;
        private int _currentChaseTicks;

        private void Start()
        {
            SetConfig();
        }

        public override void SetConfig()
        {
            base.SetConfig();
            _model = _animations.GetModelObject();

            _collider = GetComponent<CapsuleCollider>();
            _passiveHandler.SetUp(_animations.GetModelObject());


            switch (LevelBase.CurrentSave.Difficulty)
            {
                case Difficulty.Easy:
                    _decisionInterval = 1f;
                    _acuracyMultiplier = 1f;
                    break;
                case Difficulty.Normal:
                    _decisionInterval = 0.75f;
                    _acuracyMultiplier = 0.75f;
                    break;
                case Difficulty.Hard:
                    _decisionInterval = 0.5f;
                    _acuracyMultiplier = 0.5f;
                    break;
                case Difficulty.KeruMustDie:
                    _decisionInterval = 0.2f;
                    _acuracyMultiplier = 0.25f;
                    break;
            }

            _weaponHandler.SetAccuracyMultiplier(_acuracyMultiplier);

            _maxChaseTicks = Mathf.RoundToInt(30 / _decisionInterval);
            _target = PlayerBase.Singleton.transform;
        }

        protected override void Update()
        {
            base.Update();

            if (_alive)
            {
                if (_isShooting)
                {
                    var direction = (_target.position - transform.position).normalized;
                    _weaponHandler.Shoot(direction);
                    UpdateModelDirection(direction);
                }
                else
                {
                    UpdateModelDirection(_agent.velocity.normalized);
                    _weaponHandler.StopShooting();
                }
            }
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
            ExecuteNewAction(GetNewAction());
        }

        private AiActions GetNewAction()
        {
            if (_currentAction == AiActions.RELOAD)
            {
                if (_weaponHandler.HasBulletsInMag())
                {
                    return AiActions.CHASE_TARGET;
                }
            }

            if (!_weaponHandler.HasBulletsInMag())
            {
                if (_weaponHandler.HasBulletsInTotal())
                {
                    return AiActions.RELOAD;
                }

                if (_weaponHandler.GetCurrentWeaponSlot() == WeaponSlot.SECONDARY)
                {
                    _weaponHandler.RefillSecondaryAmmo();
                    return AiActions.RELOAD;
                }
                else
                {
                    _weaponHandler.DeployWeapon(WeaponSlot.SECONDARY);
                    return _currentAction;
                }
            }

            var detectedTarget = DetectTarget();

            if (_weaponHandler.CanShoot() && detectedTarget.detected && detectedTarget.distance <= _npcStats.AttackDistance)
            {
                _currentChaseTicks = 0;
                return AiActions.ATTACK;
            }
            else if((_weaponHandler.CanShoot() && detectedTarget.detected && detectedTarget.distance <= _npcStats.ViewDistance) || ((_currentAction == AiActions.ATTACK || _currentAction == AiActions.CHASE_TARGET) && _currentChaseTicks < _maxChaseTicks))
            {
                _currentChaseTicks++;
                return AiActions.CHASE_TARGET;
            }

            if (_patrolPoints != null)
            {
                _currentChaseTicks = 0;
                return AiActions.PATROL;
            }

            return AiActions.IDLE;
        }

        private void ExecuteNewAction(AiActions newAction)
        {
            switch (newAction)
            {
                case AiActions.IDLE:
                    Idle();
                    break;
                case AiActions.PATROL:
                    Patrol();
                    break;
                case AiActions.ATTACK:
                    Attack();
                    break;
                case AiActions.RELOAD:
                    Reload();
                    break;
                case AiActions.TAKE_COVER:
                    //SeekCover();
                    break;
                case AiActions.CHASE_TARGET:
                    ChasePlayer();
                    break;
                case AiActions.DIE:
                    _agent.isStopped = true;
                    break;
            }

            _currentAction = newAction;
        }

        private void Idle()
        {
            _isShooting = false;
        }

        private (float distance, bool detected) DetectTarget()
        {
            var dirToTarget = (_target.transform.position - transform.position).normalized;
            var angleToTarget = Vector3.Angle(_model.transform.forward, dirToTarget);
            var distanceToTarget = Vector3.Distance(transform.position, _target.transform.position);

            if (angleToTarget <= _npcStats.AngleVision && distanceToTarget <= _npcStats.ViewDistance)
            {
                RaycastHit rch;
                if (Physics.Raycast(transform.position, dirToTarget, out rch, distanceToTarget))
                {
                    if (rch.collider.gameObject == _target.gameObject)
                    {
                        return (distanceToTarget, true);
                    }
                    else
                    {
                        return (distanceToTarget, false);
                    }
                }
            }

            return (distanceToTarget, false);
        }

        private void Patrol()
        {
            _isShooting = false;
            _agent.isStopped = false;
            if (_patrolPoints != null)
            {
                _agent.SetDestination(_patrolPoints.GetCurrentWaypoint());
            }
        }

        private void Attack()
        {
            _isShooting = true;
            _agent.isStopped = false;
            _agent.SetDestination(_target.position);
        }

        private void Reload()
        {
            _isShooting = false;
            _agent.isStopped = true;
            _weaponHandler.Reload();
        }

        private void ChasePlayer()
        {
            _isShooting = false;
            _agent.isStopped = false;
            _agent.SetDestination(_target.position);
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
            _weaponHandler.Die();

            if (_patrolPoints != null)
            {
                _patrolPoints.Die();
            }

            ExecuteNewAction(AiActions.DIE);
            Destroy(gameObject, 60f);
        }
    }
}
