using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Keru.Scripts.Game.Cutscene
{
    public class CutsceneTitleScreen : MonoBehaviour
    {
        [SerializeField] private Text _titleText;
        [SerializeField] private Text _subTitleText;

        private Animator _blackScreenAnimator;

        private void Start()
        {
            _blackScreenAnimator = GetComponent<Animator>();
        }

        public void EnableTitleScreen(string title, string subTitle, GameObject continueButton)
        {
            _blackScreenAnimator.Play("ChangeSceneUp");

            StartCoroutine(UpdateText(title, subTitle, continueButton));
        }

        private IEnumerator UpdateText(string title, string subTitle, GameObject continueButton)
        {
            yield return new WaitForSeconds(0.5f);
       
            if (title != string.Empty)
            {
                var titleAux = "";
                while (!titleAux.Equals(title))
                {
                    var titleTime = 1.5f / title.Length;
                    titleAux = title.Substring(0, titleAux.Length + 1);
                    _titleText.text = titleAux.ToUpper();
                    yield return new WaitForSeconds(titleTime * Time.deltaTime);
                }
            }

            yield return new WaitForSeconds(0.5f);

            if (subTitle != string.Empty)
            {
                var subTitleTime = 1.5f / subTitle.Length;
                var subTitleAux = "";
                while (!subTitleAux.Equals(subTitle))
                {
                    subTitleAux = subTitle.Substring(0, subTitleAux.Length + 1);
                    _subTitleText.text = subTitleAux.ToUpper();
                    yield return new WaitForSeconds(subTitleTime * Time.deltaTime);
                }
            }

            continueButton.SetActive(true);
        }

        public void DisableTitleScreen(GameObject continueButton = null)
        {
            _blackScreenAnimator.Play("ChangeScene");
            StartCoroutine(RemoveText(continueButton));
        }

        private IEnumerator RemoveText(GameObject continueButton)
        {
            if (_titleText.text != string.Empty)
            {
                var titleAux = _titleText.text;
                var titleTime = 0.5f / titleAux.Length;
                while (titleAux.Length > 0)
                {
                    titleAux = titleAux.Substring(0, titleAux.Length - 1);
                    _titleText.text = titleAux.ToUpper();
                    yield return new WaitForSeconds(titleTime * Time.deltaTime);
                }
            }

            if (_subTitleText.text != string.Empty)
            {
                var subTitleAux = _subTitleText.text;
                var subTitleTime = 0.5f / subTitleAux.Length;
                while (subTitleAux.Length > 0)
                {
                    subTitleAux = subTitleAux.Substring(0, subTitleAux.Length - 1);
                    _subTitleText.text = subTitleAux.ToUpper();
                    yield return new WaitForSeconds(subTitleTime * Time.deltaTime);
                }
            }

            if (continueButton != null)
            {
                continueButton.SetActive(true);
            }
        }
    }
}