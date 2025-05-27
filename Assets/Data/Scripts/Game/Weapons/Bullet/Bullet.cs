using Keru.Game.Actions;
using Keru.Scripts.Game;
using Keru.Scripts.Game.Entities;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private Action _action;
    [SerializeField] private DamageType _damageType;
    [SerializeField] private bool _usesGravity;
    [SerializeField] private bool _affectsPlayer;

    private int _damage;
    private float _force;
    private GameObject _owner;

    public virtual void SetUp(int damage, float force, GameObject owner)
    {
        _damage = damage;
        _force = force;
        _owner = owner;

        var rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * _speed;
        rb.useGravity = _usesGravity;

        if(_action != null)
        {
            _action.SetUp(_damage, _force, string.Empty, _owner, _affectsPlayer);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        var entity = collision.gameObject.GetComponent<Entity>();

        if (entity != null)
        {
            entity.OnDamagedUnit(_damage, collision.contacts[0].point, _owner, _damageType, _force);
        }

        if (_action != null)
        {
            _action.Execute();
        }

        Destroy(gameObject);
    }
}
