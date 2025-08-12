using Keru.Scripts.Game;
using Keru.Scripts.Game.Entities;
using Keru.Scripts.Game.Entities.Humanoid;
using Keru.Scripts.Game.Entities.NPCs;
using Keru.Scripts.Game.Entities.Utils;
using Keru.Scripts.Game.ScriptableObjects;
using UnityEngine;
using UnityEngine.AI;

public class NPC : Entity
{
    [Header("NPC Settings")]
    [SerializeField] protected NPCStats _npcStats;
    [SerializeField] protected Waypoint _patrolPoints;
    private float _maxSpeed = 3f;

    protected NavMeshAgent _agent;
    protected Transform _target;
    protected AiActions _currentAction;
    protected float _decisionInterval = 0.3f;
    protected float _acuracyMultiplier = 1f;
    protected float _timeThinking;

    protected NPCWeaponHandler _weaponHandler;
    protected ThirdPersonAnimations _animations;

    public virtual void SetConfig()
    {
        _maxLife = _npcStats.MaxLife;
        _life = _maxLife;
        _maxSpeed = _npcStats.MaxSpeed;

        _agent = gameObject.AddComponent<NavMeshAgent>();
        _agent.baseOffset = 1;
        _agent.updateRotation = false;
        _agent.stoppingDistance = 0.1f;
        _agent.speed = _maxSpeed;

        if (_patrolPoints != null)
        {
            _patrolPoints.SetConfig(this);
        }

        _animations = GetComponent<ThirdPersonAnimations>();
        _animations.SetConfig();

        _weaponHandler = GetComponent<NPCWeaponHandler>();
        _weaponHandler.SetConfig(_animations, _npcStats);

    }

    protected virtual void Update()
    {
        if (!_alive)
        {
            return;
        }

        _timeThinking += Time.deltaTime;
        if (_timeThinking >= _decisionInterval)
        {
            _timeThinking = 0f;
            Think();
        }
    }

    protected virtual void Think()
    {
        // Override this method in derived classes to implement specific AI behavior
    }
}
