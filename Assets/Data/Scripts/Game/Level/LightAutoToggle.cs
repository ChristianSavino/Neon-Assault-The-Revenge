using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

namespace Keru.Scripts.Game.Level
{
    public class LightAutoToggle : MonoBehaviour
    {
        [SerializeField] private Material _lightsOn;
        [SerializeField] private Material _lightsOff;
        [SerializeField] private float _changeLightDelay;
        [SerializeField] private List<RendererList> _renderers;
        [SerializeField] private GameObject _light;
        [SerializeField] private List<Vector3> _lightPosition;

        private int _counter;

        private void Start()
        {
            foreach (var renderers in _renderers)
            {
                foreach(var renderer in renderers.Renderers)
                {
                    renderer.material = _lightsOff;
                }              
            }

            ToggleLights(_lightsOn, _counter);
            StartCoroutine(ToggleLights());
        }

        private IEnumerator ToggleLights()
        {
            while (true)
            {
                yield return new WaitForSeconds(_changeLightDelay);

                ToggleLights(_lightsOff, _counter);
                _counter++;

                if (_counter >= _renderers.Count)
                {
                    _counter = 0;
                }

                ToggleLights(_lightsOn, _counter);
            }
        }

        private void ToggleLights(Material material, int counter)
        {
            foreach (var renderer in _renderers[counter].Renderers)
            {
                renderer.material = material;
            }

            _light.transform.position = _lightPosition[counter];
        }
    }

    [Serializable]
    public class RendererList
    {
        public List<Renderer> Renderers;
    }
}