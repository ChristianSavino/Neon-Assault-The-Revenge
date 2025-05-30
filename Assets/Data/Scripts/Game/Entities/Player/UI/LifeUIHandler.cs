using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;


namespace Keru.Scripts.Game.Entities.Player.UI
{
    public class LifeUIHandler : MonoBehaviour
    {
        [Header("Life UI Elements")]
        [SerializeField] private Image _lifeBar;
        [SerializeField] private Image _coloredLifeBar;
        [SerializeField] private Text _lifeText;
        [SerializeField] private Animator _hitAnimation;
        [SerializeField] private Volume _volume;
        [SerializeField] private float _minimumLifeWhiteScreen = 0.5f;

        [Header("Armor")]
        [SerializeField] private GameObject _armorBox;
        [SerializeField] private Text _armorText;

        private int _maxLife;
        private Coroutine _redBarCoroutine;

        public void SetConfig(int life, int maxLife)
        {
            _maxLife = maxLife;
            var currentLife = (float)life / _maxLife;
            _lifeBar.fillAmount = currentLife;
            _coloredLifeBar.fillAmount = currentLife;
            _lifeText.text = life.ToString();
        }

        public void SetLife(int life, bool isHealing = false)
        {
            var currentLife = (float)life / _maxLife;
            _lifeText.text = life.ToString();
            CalculateVolumeWeight(currentLife);
            if (isHealing)
            {
                _hitAnimation.Play("Heal");
                _coloredLifeBar.color = Color.green;
                _coloredLifeBar.fillAmount = currentLife;
                if (_redBarCoroutine != null)
                {
                    StopCoroutine(_redBarCoroutine);
                }

                _redBarCoroutine = StartCoroutine(AnimateBar(currentLife, _lifeBar));
            }
            else
            {
                _hitAnimation.Play("Hit");
                _coloredLifeBar.color = Color.red;
                _lifeBar.fillAmount = currentLife;
                if (_redBarCoroutine != null)
                {
                    StopCoroutine(_redBarCoroutine);
                }

                _redBarCoroutine = StartCoroutine(AnimateBar(currentLife, _coloredLifeBar));
            }
        }

        public void SetArmor(int armor)
        {
            _armorBox.SetActive(armor > 0);
            _armorText.text = armor.ToString();
        }

        private IEnumerator AnimateBar(float targetFill, Image fillImage)
        {
            yield return new WaitForSeconds(1f);

            float duration = 0.5f;
            float elapsed = 0f;
            float startFill = _coloredLifeBar.fillAmount;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                _coloredLifeBar.fillAmount = Mathf.Lerp(startFill, targetFill, elapsed / duration);
                yield return null;
            }

            _coloredLifeBar.fillAmount = targetFill;
        }

        private void CalculateVolumeWeight(float currentLife)
        {
            if (currentLife >= _minimumLifeWhiteScreen)
            {
                _volume.weight = 0f;
            }
            else
            {
                _volume.weight = (_minimumLifeWhiteScreen - currentLife) / _minimumLifeWhiteScreen;
            }
        }

        public void Die()
        {
            _volume.weight = 1;
        }
    }
}
