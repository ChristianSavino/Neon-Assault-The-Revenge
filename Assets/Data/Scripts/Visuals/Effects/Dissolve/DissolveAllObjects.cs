using Keru.Scripts.Engine.Module;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Keru.Scripts.Visuals.Effects.Dissolve
{
    public class DissolveAllObjects : MonoBehaviour
    {
        private float _dissolveDuration = 2f;
        private Material _dissolveMaterial;
        private Color _dissolveColor;
        private IEnumerable<Renderer> _renderers;

        public void SetConfig(Color color)
        {
            _dissolveMaterial = CommonItemsManager.ItemsManager.DissolveMaterial;
            _dissolveColor = color;

            _renderers = transform.GetComponentsInChildren<Renderer>()
                .Where(x => x != null && x.gameObject.activeSelf && x.gameObject.GetComponent<ParticleSystem>() == null);

            foreach (var renderer in _renderers)
            {
                if (renderer.gameObject.activeSelf)
                {
                    renderer.material = _dissolveMaterial;
                    renderer.material.SetColor("_Edge_Color", _dissolveColor);
                }
            }

            Execute();
        }

        private void Execute()
        {
            StartCoroutine(DissolveObject());
        }

        private IEnumerator DissolveObject()
        {
            var elapsedTime = 0f;
            while (elapsedTime < _dissolveDuration)
            {
                elapsedTime += Time.deltaTime;
                foreach (var renderer in _renderers)
                {
                    var dissolveAmount = Mathf.Clamp01(elapsedTime / _dissolveDuration);
                    renderer.material.SetFloat("_Dissolve", dissolveAmount);
                }

                yield return null;
            }

            Destroy(gameObject);
        }
    }
}
