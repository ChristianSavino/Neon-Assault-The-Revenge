using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Keru.Scripts.Game.Menus
{
    public class MenuConsole : MonoBehaviour
    {
        public static MenuConsole menuConsole;
        
        [SerializeField] private Text _message;
        [SerializeField] private Text _clock;
        [SerializeField] private Text _version;

        private Animator _messageAnimator;

        private void Start()
        {
            menuConsole = this;
            
            _messageAnimator = _message.GetComponent<Animator>();
            _version.text = Application.version;
        }

        private void Update()
        {
            var currentTime = DateTime.Now;
            _clock.text = currentTime.ToString("HH:mm:ss");
        }

        public void Message(string message)
        {
            _message.text = message.ToUpper();
            _messageAnimator.Play("Message");
        }
    }
}