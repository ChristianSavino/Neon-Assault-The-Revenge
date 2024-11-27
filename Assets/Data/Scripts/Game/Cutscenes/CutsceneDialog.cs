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
        [SerializeField] private float _dialogSpeed = 0.05f;
        [SerializeField] private bool _usesUpperCase = true;

        public void LoadDialog(string characterName, string dialog, GameObject continueButton)
        {
            StartCoroutine(ChangeDialog(characterName, dialog, continueButton));
        }

        private IEnumerator ChangeDialog(string characterName, string dialog, GameObject continueButton)
        {          
            _characterBox.text = characterName.ToUpper();

            var dialogAux = "";
            while (!dialogAux.Equals(dialog))
            {
                dialogAux = dialog.Substring(0, dialogAux.Length + 1);
                
                if(_usesUpperCase)
                {
                    _characterDialog.text = dialogAux.ToUpper();
                }
                else
                {
                    _characterDialog.text = dialogAux;
                }

                yield return new WaitForSeconds(_dialogSpeed);
            }

            continueButton.SetActive(true);
        }
    }
}