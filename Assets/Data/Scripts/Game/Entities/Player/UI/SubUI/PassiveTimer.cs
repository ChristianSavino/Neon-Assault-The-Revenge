using Keru.Scripts.Game.ScriptableObjects;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Keru.Scripts.Game.Entities.Player.UI
{
    public class PassiveTimer : MonoBehaviour
    {
        [SerializeField] private GameObject _container;
        [SerializeField] private Text _passiveText;
        [SerializeField] private Image _image;
        
        private float _timer;
        private bool _usesRealTime;
        private PassiveUIHandler _handler;

        public void SetConfig(PassiveStats stats, PassiveUIHandler handler)
        {
            _container.SetActive(true);

            _handler = handler;
            _timer = stats.Duration;
            _image.sprite = stats.Icon;
            _image.color = stats.Potivity == PassivePotivity.POSITIVE ? Color.cyan : Color.red;


            _usesRealTime = stats.UsesRealTime;
            StartCoroutine(RunPassive());
        }

        private IEnumerator RunPassive()
        {
            while (_timer > 0)
            {      
                _passiveText.text = _timer.ToString();

                if (_usesRealTime)
                {
                    yield return new WaitForSecondsRealtime(1);
                }
                else
                {
                    yield return new WaitForSeconds(1);
                }

                _timer--;
            }

            _handler.PassiveDestroy();
            Destroy(gameObject);
        }
    }
}
