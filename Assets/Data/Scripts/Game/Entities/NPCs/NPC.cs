using Keru.Scripts.Game;
using Keru.Scripts.Game.Entities;
using Keru.Scripts.Game.Entities.Utils;
using UnityEngine;
using UnityEngine.AI;

public class NPC : Entity
{
    [Header("NPC Settings")]
    [SerializeField] protected Waypoint _patrolPoints;
    [SerializeField] private float _speed = 3f;

    protected NavMeshAgent _agent;
    protected Transform _target;
    protected AiActions _currentAction;
    protected float _decisionInterval = 0.3f;
    protected float _acuracyMultiplier = 1f;
    protected float _timeThinking;

    public virtual void SetConfig()
    {
        _agent = gameObject.AddComponent<NavMeshAgent>();
        _agent.baseOffset = 1;
        _agent.updateRotation = false;
        _agent.stoppingDistance = 0.1f;
        _agent.speed = _speed;

        if (_patrolPoints != null)
        {
            _patrolPoints.SetConfig(this);
        }
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

    public virtual void SetPatrolDestination(Vector3 destination)
    {
        if (_agent != null && _agent.enabled)
        {
            _agent.SetDestination(destination);
        }
    }

    protected virtual void Think()
    {
        // Override this method in derived classes to implement specific AI behavior
    }
}
