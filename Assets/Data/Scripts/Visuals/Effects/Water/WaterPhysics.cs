using Keru.Scripts.Game.Entities;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Keru.Scripts.Visuals.Effects.Water
{
    public class WaterPhysics : MonoBehaviour
    {
        [SerializeField] private GameObject _ripplePrefab;
        [SerializeField] private GameObject _splashPrefab;
        [SerializeField] private GameObject _splashMiniPrefab;
        [SerializeField] private float _depthBeforeSubmerged;
        [SerializeField] private float _displacementAmmount;

        private Dictionary<GameObject, GameObject> _objectsWithRipples;

        void Start()
        {
            _objectsWithRipples = new Dictionary<GameObject, GameObject>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.gameObject.isStatic)
            {
                var gameObject = CheckIfGameObjectIsEntity(other);
                
                if(!_objectsWithRipples.TryGetValue(gameObject, out var aux))
                {
                    var ripple = Instantiate(_ripplePrefab, gameObject.transform);
                    ripple.transform.position = gameObject.transform.position;
                    ripple.transform.localScale = Vector3.one * other.bounds.size.x;

                    _objectsWithRipples.Add(gameObject, ripple);
                }

                if (other.material.name.Contains("Floaty"))
                {
                    var floatyObject = other.gameObject.AddComponent<FloatyObject>();
                    if (other.material.name.Contains("Surface"))
                    {
                        floatyObject.Setup(_depthBeforeSubmerged, _displacementAmmount, transform.position.y + (other.bounds.size.y * 0.45f));
                    }
                    else
                    {
                        floatyObject.Setup(_depthBeforeSubmerged, _displacementAmmount, transform.position.y);
                    }
                }
            }
        }

        private GameObject CheckIfGameObjectIsEntity(Collider other)
        {
            if (other.gameObject.GetComponent<Entity>() || other.gameObject.layer == LayerMask.NameToLayer("Ragdoll"))
            {
                return (from x in other.gameObject.transform.root.GetComponentsInChildren<Transform>()
                        where x.gameObject.name == "Hips"
                        select x.gameObject).First();
            }

            return other.gameObject;
        }

        private void OnTriggerExit(Collider other)
        {
            var gameObject = CheckIfGameObjectIsEntity(other);

            if (_objectsWithRipples.TryGetValue(gameObject, out var ripple))
            {
                _objectsWithRipples.Remove(gameObject);
                Destroy(ripple);
            }

            if (other.material.name.Contains("Floaty"))
            {
                Destroy(gameObject.GetComponent<FloatyObject>());
            }
        }
    }
}