using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Keru.Scripts.Game.Menus
{
    public class TitleScreen : MonoBehaviour
    {
        [SerializeField] private GameObject _mainMenu;

        public void Update()
        {
            if (Input.anyKeyDown)
            {
                _mainMenu.SetActive(true);
                gameObject.SetActive(false);
            }
        }
    }
}
