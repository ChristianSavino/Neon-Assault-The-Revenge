using UnityEngine;

namespace Keru.Scripts.Game.Entities
{
    public class Entity : MonoBehaviour
    {
        [SerializeField] protected int _life;
        [SerializeField] protected bool _alive = true;

        public virtual void OnDamagedUnit(int damage, Vector3 hitpoint, GameObject origin, DamageType damageType, float damageForce)
        {

        }

        public virtual void ForceDeployWeapon()
        {

        }
    }
}
