using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Keru.Scripts.Game.Cutscene
{
    public class CutsceneDialog : MonoBehaviour
    {
        [Header("Objects")]
        [SerializeField] private Text _characterBox;
        [SerializeField] private Text _characterDialog;

        [Header("Dialog")]
        [SerializeField] private List<string> _characterString;
        [SerializeField] private List<string> _dialogString;

        private int _currentDialog;

        private void Start()
        {
            StartCoroutine(ChangeDialog());
        }

        public void ContinueDialog()
        {
            StartCoroutine(ChangeDialog());
        }

        private IEnumerator ChangeDialog()
        {
            var timeToAddLetter = 2f / _dialogString[_currentDialog].Length;
            var dialog = _dialogString[_currentDialog];
            _characterBox.text = _characterString[_currentDialog];

            var dialogAux = "";
            while (!dialogAux.Equals(dialog))
            {
                dialogAux = dialog.Substring(0, dialogAux.Length + 1);
                _characterDialog.text = dialogAux.ToUpper();
                yield return new WaitForSeconds(timeToAddLetter);
            }


        }
    }
}