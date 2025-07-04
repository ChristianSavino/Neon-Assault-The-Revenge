using UnityEngine;

namespace Keru.Scripts.Game.Objects.Pickups
{
    [RequireComponent(typeof(BoxCollider))]
    public class Pickup : MonoBehaviour
    {
        [SerializeField] protected AudioClip _soundToPlay;
        [SerializeField] protected bool _objectSpins;

        private void Start()
        {
            if (_objectSpins)
            {
                var spin = transform.Find("Spin").gameObject.GetComponent<Animator>();
                spin.enabled = true;
            }
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
            {
                return;
            }
        }
    }
}
