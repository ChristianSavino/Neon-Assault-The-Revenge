using Keru.Scripts.Engine.FileSystem;
using Keru.Scripts.Engine.Master;
using UnityEngine;
using UnityEngine.UI;

namespace Keru.Scripts.Game.Menus.SubMenus
{
    public class GameplayOptionsSubMenu : MonoBehaviour
    {
        private ControlMapping _controlMapping;
        private GameObject _currentKey;

        [Header("Sensibility")]
        [SerializeField] private Slider _sensibilitySlider;
        [SerializeField] private Text _sensibilityAmmount;

        [Header("Controls")]
        [SerializeField] private Text _up;
        [SerializeField] private Text _left, _right, _back, _dash, _jump, _duck, _shoot, _reload, _flashlight, _special, _second, _pause, _use, _grenade, _knife, _autoReload;

        private void OnEnable()
        {
            _controlMapping = LevelBase.GameOptions.Options.PlayerControls;

            _up.text = _controlMapping.Keys["Up"].ToString().ToUpper();
            _left.text = _controlMapping.Keys["Left"].ToString().ToUpper();
            _right.text = _controlMapping.Keys["Right"].ToString().ToUpper();
            _back.text = _controlMapping.Keys["Back"].ToString().ToUpper();
            _dash.text = _controlMapping.Keys["Dash"].ToString().ToUpper();
            _jump.text = _controlMapping.Keys["Jump"].ToString().ToUpper();
            _duck.text = _controlMapping.Keys["Duck"].ToString().ToUpper();
            _shoot.text = _controlMapping.Keys["Shoot"].ToString().ToUpper();
            _reload.text = _controlMapping.Keys["Reload"].ToString().ToUpper();
            _flashlight.text = _controlMapping.Keys["Flashlight"].ToString().ToUpper();
            _special.text = _controlMapping.Keys["Special"].ToString().ToUpper();
            _second.text = _controlMapping.Keys["Second"].ToString().ToUpper();
            _pause.text = _controlMapping.Keys["Pause"].ToString().ToUpper();
            _use.text = _controlMapping.Keys["Use"].ToString().ToUpper();
            _knife.text = _controlMapping.Keys["Knife"].ToString().ToUpper();
            _grenade.text = _controlMapping.Keys["Grenade"].ToString().ToUpper();
            _sensibilitySlider.value = _controlMapping.Sensibility * 100;
            _autoReload.text = _controlMapping.AutoReload ? "ACTIVADO" : "DESACTIVADO";

            UpdateSensibility();
        }

        public void UpdateSensibility()
        {
            _controlMapping.Sensibility = _sensibilitySlider.value / 100f;
            _sensibilityAmmount.text = _sensibilitySlider.value.ToString();
        }

        public void AutoReload()
        {
            _controlMapping.AutoReload = !_controlMapping.AutoReload;
            _autoReload.text = _controlMapping.AutoReload ? "ACTIVADO" : "DESACTIVADO";
        }

        public void ChangeKey(GameObject clicked)
        {
            _currentKey = clicked;
        }

        public void SaveControls()
        {
            ExternalFilesManager.UpdateGameData(LevelBase.GameOptions);
            MenuConsole.menuConsole.Message("Controles Guardados");
            gameObject.SetActive(false);
        }

        private void OnGUI()
        {
            if(_currentKey != null)
            {
                var e = Event.current;
                if (e.type == EventType.KeyDown || e.type == EventType.MouseDown)
                {
                    if (!IsKeyAssigned(e))
                    {
                        if (e.isKey)
                        {
                            if (validateKeyControls(e))
                            {
                                _controlMapping.Keys[_currentKey.name] = e.keyCode;
                                _currentKey.transform.GetChild(0).GetComponent<Text>().text = e.keyCode.ToString().ToUpper();
                                _currentKey = null;
                            }
                            else
                            {
                                MenuConsole.menuConsole.Message("Esta tecla no se puede asignar".ToUpper());
                            }
                        }
                        else if (e.isMouse)
                        {
                            if (validateMouseWheel(e))
                            {
                                var mouse = e.button;
                                var mouseB = (KeyCode)System.Enum.Parse(typeof(KeyCode), "Mouse" + mouse);
                                _controlMapping.Keys[_currentKey.name] = mouseB;
                                _currentKey.transform.GetChild(0).GetComponent<Text>().text = mouseB.ToString().ToUpper();
                                _currentKey = null;
                            }
                            else
                            {
                                MenuConsole.menuConsole.Message("La Rueda del Mouse no se puede asignar");
                            }
                        }
                    }
                    else
                    {
                        MenuConsole.menuConsole.Message("Tecla ya asignada");
                    }
                }
            }
        }

        private bool IsKeyAssigned(Event e)
        {
            var isAssigned = false;

            foreach (var item in _controlMapping.Keys)
            {
                if(e.isKey)
                {
                    if (item.Value == e.keyCode)
                    {
                        isAssigned = true;
                    }
                }
                else
                {
                    var mouse = e.button;
                    var mouseB = (KeyCode)System.Enum.Parse(typeof(KeyCode), "Mouse" + mouse);
                    if (item.Value == mouseB)
                    {
                        isAssigned = true;
                    }
                }
            }

            return isAssigned;
        }

        private bool validateKeyControls(Event e)
        {
            var isValid = true;
            switch (e.keyCode)
            {
                case KeyCode.Alpha0:
                case KeyCode.Alpha1:
                case KeyCode.Alpha2:
                case KeyCode.Alpha3:
                case KeyCode.Alpha4:
                case KeyCode.Alpha5:
                case KeyCode.Alpha6:
                case KeyCode.Alpha7:
                case KeyCode.Alpha8:
                case KeyCode.Alpha9:
                    isValid = false;
                    break;
            }
            return isValid;
        }

        private bool validateMouseWheel(Event e)
        {
            var isValid = true;
            if (e.isScrollWheel)
                isValid = false;

            return isValid;
        }
    }
}