using Keru.Scripts.Engine.Module;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

namespace Keru.Game.Effects.AfterImages
{
    public class AfterImage : MonoBehaviour
    {
        [SerializeField] private float _lifetime = 0.5f;
        [SerializeField] private float _fadeSpeed = 2f;
        private List<Renderer> _renderers;

        public void SetConfig(float lifetime, float fadeSpeed)
        {
            _lifetime = lifetime;
            _fadeSpeed = fadeSpeed;
            _renderers = GetComponentsInChildren<Renderer>().ToList();
            foreach (var renderer in _renderers)
            {
                renderer.material = CommonItemsManager.ItemsManager.PlayerAfterImageMaterial;
                renderer.shadowCastingMode = ShadowCastingMode.Off;
                renderer.receiveShadows = false;
            }
            StartCoroutine(AfterImageCoroutine());
        }

        private IEnumerator AfterImageCoroutine()
        {
            float elapsedTime = 0f;
            while (elapsedTime < _lifetime)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsedTime / _lifetime * _fadeSpeed);
                foreach (var renderer in _renderers)
                {
                    if (renderer.material.HasProperty("_Color"))
                    {
                        var color = renderer.material.color;
                        color.a = alpha;
                        renderer.material.color = color;
                    }
                }
                yield return null;
            }
            Destroy(gameObject);
        }
    }
}