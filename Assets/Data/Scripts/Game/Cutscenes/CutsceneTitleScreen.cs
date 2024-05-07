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

        public void EnableTitleScreen(string title, string subTitle)
        {
            _blackScreenAnimator.Play("ChangeSceneUp");

            StartCoroutine(UpdateText(title, subTitle));
        }

        private IEnumerator UpdateText(string title, string subTitle)
        {
            yield return new WaitForSeconds(1);
            
            if (title != string.Empty)
            {
                var titleAux = "";
                while (!titleAux.Equals(title))
                {
                    titleAux = title.Substring(0, titleAux.Length + 1);
                    _titleText.text = titleAux.ToUpper();
                    yield return new WaitForSeconds(0.1f);
                }
            }

            yield return new WaitForSeconds(0.5f);

            if (subTitle != string.Empty)
            {
                var subTitleAux = "";
                while (!subTitleAux.Equals(subTitle))
                {
                    subTitleAux = subTitle.Substring(0, subTitleAux.Length + 1);
                    _subTitleText.text = subTitleAux.ToUpper();
                    yield return new WaitForSeconds(0.1f);
                }
            }
        }

        public void DisableTitleScreen()
        {
            _blackScreenAnimator.Play("ChangeScene");
            StartCoroutine(RemoveText());
        }

        private IEnumerator RemoveText()
        {
            yield return new WaitForSeconds(1);

            if (_titleText.text != string.Empty)
            {
                var titleAux = _titleText.text;
                while (titleAux.Length > 0)
                {
                    titleAux = titleAux.Substring(0, titleAux.Length - 1);
                    _titleText.text = titleAux.ToUpper();
                    yield return new WaitForSeconds(0.05f);
                }
            }

            if (_subTitleText.text != string.Empty)
            {
                var subTitleAux = _subTitleText.text;
                while (subTitleAux.Length > 0)
                {
                    subTitleAux = subTitleAux.Substring(0, subTitleAux.Length - 1);
                    _subTitleText.text = subTitleAux.ToUpper();
                    yield return new WaitForSeconds(0.05f);
                }
            }
        }
    }
}