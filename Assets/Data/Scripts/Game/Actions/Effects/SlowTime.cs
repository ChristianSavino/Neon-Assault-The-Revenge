using Keru.Scripts.Engine.Master;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

namespace Keru.Game.Actions.Effects
{
    public class SlowTime : MonoBehaviour
    {
        [SerializeField] private float _slowdownFactor;
        [SerializeField] private float _slowdownLenght;
        [SerializeField] private float _recover;

        private float _originalTimeScale;
        private bool _doRecover;

        private Volume _volume;

        void Update()
        {
            if (_doRecover)
            {
                var timeToRecover = 1f / _recover * Time.unscaledDeltaTime;
                var timeScale = Time.timeScale + timeToRecover;

                SetVolumeWeight(timeToRecover);

                LevelBase.levelBase.SetTimeScale(timeScale);

                if (_originalTimeScale != LevelBase.levelBase.LocalTimeScale)
                {
                    _originalTimeScale = LevelBase.levelBase.LocalTimeScale;
                }

                if (timeScale >= _originalTimeScale)
                {
                    LevelBase.levelBase.SetTimeScale(_originalTimeScale);

                    SetVolumeWeight(0);
                    Destroy(this);
                }
            }
        }

        public void SetUp(float slowdownFactor, float slowdownLenght, float recover, Volume volume = null)
        {
            _slowdownFactor = slowdownFactor;
            _slowdownLenght = slowdownLenght;
            _recover = recover;
            _volume = volume;
        }

        public Coroutine DoSlowTime()
        {
            return StartCoroutine(BulletTime());
        }

        public IEnumerator BulletTime()
        {
            _originalTimeScale = Time.timeScale;

            LevelBase.levelBase.SetTimeScale(_slowdownFactor);

            yield return new WaitForSecondsRealtime(_slowdownLenght);

            _doRecover = true;
        }

        private void SetVolumeWeight(float weight)
        {
            if (_volume != null)
            {
                _volume.weight = weight;
            }
        }
    }
}