using UnityEngine;

namespace Keru.Scripts.Game.Weapons
{
    public class Casing : MonoBehaviour
    {
        
        private void Start()
        {
            var rb = GetComponent<Rigidbody>();
            rb.AddForce(transform.right * Random.Range(1f, 3f), ForceMode.Impulse);
        }
    }
}