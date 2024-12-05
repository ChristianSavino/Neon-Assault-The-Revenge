using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Keru.Scripts.Engine.Debug
{
    public class FpsBenchmark : MonoBehaviour
    {
        [SerializeField] private Text _fps;
        [SerializeField] private Text _minFps;
        [SerializeField] private Text _maxFps;
        [SerializeField] private Text _avgFps;

        private int[] _avgFpsCount = new int[60];

        private int _min = 9999;
        private int _max = 0;

        private int _count;

        private void Start()
        {
            _fps.text = "0";
            _minFps.text = "0";
            _maxFps.text = "0";
            _avgFps.text = "0";

            SetAvgFpsCountToZero();
            StartCoroutine(CalculateFPS());
        }
        private IEnumerator CalculateFPS()
        {
            yield return new WaitForSecondsRealtime(0.5f);

            var fps = GetFPS();
            _fps.text = fps.ToString();

            if (fps < _min)
            {
                _min = fps;
                _minFps.text = _min.ToString();
            }

            if (fps > _max)
            {
                _max = fps;
                _maxFps.text = _max.ToString();
            }
            AddFiFoNumber(fps);
            CalculateAvgFps();
            StartCoroutine(CalculateFPS());
        }

        private int GetFPS()
        {
            return (int)(1 / Time.unscaledDeltaTime);
        } 
        
        private void SetAvgFpsCountToZero()
        {
            for(int i = 0; i< _avgFpsCount.Length; i++) 
            { 
                _avgFpsCount[i] = 0; 
            }
        }

        private void AddFiFoNumber(int fps)
        {
            _avgFpsCount[_count] = fps;
            
            _count++;
            if(_count == _avgFpsCount.Length)
            {
                _count = 0;
            }
        }

        private void CalculateAvgFps()
        {
            var countedFps = _avgFpsCount.Where(x => x > 0);
            var avgFps = countedFps.Average();

            _avgFps.text = avgFps.ToString("#.##");
        }
    }
}