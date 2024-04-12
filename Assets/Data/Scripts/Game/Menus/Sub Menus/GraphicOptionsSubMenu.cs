using Keru.Scripts.Engine;
using Keru.Scripts.Engine.FileSystem;
using Keru.Scripts.Engine.Master;
using Keru.Scripts.Engine.Module;
using UnityEngine;
using UnityEngine.UI;

namespace Keru.Scripts.Game.Menus.SubMenus
{
    public class GraphicOptionsSubMenu : MonoBehaviour
    {
        [Header("Texture")]
        [SerializeField] private Slider _textureQuantity;
        [SerializeField] private Text _textureAmmount;

        [Header("Shadow")]
        [SerializeField] private Slider _shadowQuantity;
        [SerializeField] private Text _shadowAmmount;

        [Header("Field Of View")]
        [SerializeField] private Slider _fovQuantity;
        [SerializeField] private Text _fovAmmount;

        [Header("Anti-Aliasing")]
        [SerializeField] private Slider _aaQuantity;
        [SerializeField] private Text _aaAmmount;

        [Header("Anti-Aliasing Quality")]
        [SerializeField] private GameObject _aaQualityCapsule;
        [SerializeField] private Slider _aaQualityQuantity;
        [SerializeField] private Text _aaQualityAmmount;

        [Header("MSAA Quality")]
        [SerializeField] private GameObject _msaaCapsule;
        [SerializeField] private Slider _msaaQuantity;
        [SerializeField] private Text _msaaAmmount;

        [Header("Render Scale")]
        [SerializeField] private Slider _renderQuantity;
        [SerializeField] private Text _renderAmmount;

        [Header("V-Sync")]
        [SerializeField] private Button _vsyncButton;
        [SerializeField] private Text _vsyncText;

        [Header("Framer Limiter")]
        [SerializeField] private Slider _limiterQuantity;
        [SerializeField] private Text _limiterAmmount;

        [Header("Bloom")]
        [SerializeField] private Button _bloomButton;
        [SerializeField] private Text _bloomText;

        [Header("Bloom Intensify")]
        [SerializeField] private GameObject _bloomIntensifyCapsule;
        [SerializeField] private Slider _bloomIntensifyQuantity;
        [SerializeField] private Text _bloomIntensifyAmmount;

        [Header("Motion Blur")]
        [SerializeField] private Button _motionBlurButton;
        [SerializeField] private Text _motionBlurText;

        [Header("Motion Blur Intensify")]
        [SerializeField] private GameObject _motionBlurIntensifyCapsule;
        [SerializeField] private Slider _motionBlurIntensifyQuantity;
        [SerializeField] private Text _motionBlurIntensifyAmmount;

        [Header("Dithering")]
        [SerializeField] private Button _ditheringButton;
        [SerializeField] private Text _ditheringText;

        [Header("Depth Of Field")]
        [SerializeField] private Button _dofButton;
        [SerializeField] private Text _dofText;

        [Header("Ambient Occlusion")]
        [SerializeField] private Button _aoButton;
        [SerializeField] private Text _aoText;

        private GraphicsOptions _graphicsOptions;

        private void OnEnable()
        {
            _graphicsOptions = LevelBase.GameOptions.Options.GraphicsOptions;

            _textureQuantity.value = (int)_graphicsOptions.TextureQuality;
            _shadowQuantity.value = (int)_graphicsOptions.ShadowQuality;
            _fovQuantity.value = _graphicsOptions.FieldOfView;
            _aaQuantity.value = (int)_graphicsOptions.AaMode;
            _aaQualityQuantity.value = (int)_graphicsOptions.AaQuality;
            _msaaQuantity.value = (int)_graphicsOptions.MsaaSampleCount;
            _renderQuantity.value = _graphicsOptions.RenderScale * 100;
            _vsyncText.text = BooleanToName(_graphicsOptions.Vsync);
            _limiterQuantity.value = (int)_graphicsOptions.TargetFrame;

            _bloomText.text = BooleanToName(_graphicsOptions.BloomEnabled);
            _bloomIntensifyQuantity.value = (int)_graphicsOptions.BloomIntensity * 100;
            _motionBlurText.text = BooleanToName(_graphicsOptions.MotionblurEnabled);
            _motionBlurIntensifyQuantity.value = _graphicsOptions.MotionblurIntensity * 100;
            _ditheringText.text = BooleanToName(_graphicsOptions.Dithering);
            _dofText.text = BooleanToName(_graphicsOptions.DepthOfFieldEnabled);
            _aoText.text = BooleanToName(_graphicsOptions.AmbientOclussion);

            UpdateTexture();
            UpdateShadows();
            UpdateFieldOfView();
            UpdateAAMode();
            UpdateAAQuality();
            UpdateMSAAQuality();
            UpdateRenderScale();
            UpdateFrameLimiter();

            UpdateBloomIntensity();
            UpdateMotionBlurIntensity();
        }

        public void UpdateTexture()
        {
            var currentOption = (GraphicOptionsEnum)_textureQuantity.value;
            
            _textureAmmount.text = GetCorrectGraphicOptionsName(currentOption);
            _graphicsOptions.TextureQuality = currentOption;
        }

        public void UpdateShadows()
        {
            var currentOption = (GraphicOptionsEnum)_shadowQuantity.value;

            _shadowAmmount.text = GetCorrectGraphicOptionsName(currentOption);
            _graphicsOptions.ShadowQuality = currentOption;
        }

        public void UpdateFieldOfView()
        {
            _fovAmmount.text = _fovQuantity.value.ToString();
            _graphicsOptions.FieldOfView = (int)_fovQuantity.value;
        }

        public void UpdateAAMode()
        {
            var currentOption = (AAMode)_aaQuantity.value;

            _aaAmmount.text = currentOption == AAMode.None ? "NINGUNO" : currentOption.ToString();
            _graphicsOptions.AaMode = currentOption;

            if (currentOption == AAMode.TAA || currentOption == AAMode.SMAA)
            {
                if (currentOption == AAMode.TAA)
                {
                    _msaaQuantity.value = 0;
                   
                    UpdateMSAAQuality();
                    _msaaCapsule.SetActive(false);
                }
                else
                {
                    _msaaCapsule.SetActive(true);
                }

                _aaQualityCapsule.SetActive(true);
            }
            else
            {
                _aaQualityCapsule.SetActive(false);
            }
        }

        public void UpdateAAQuality()
        {
            var currentOption = (GraphicOptionsEnum)_aaQualityQuantity.value;

            _aaQualityAmmount.text = GetCorrectGraphicOptionsName(currentOption);
            _graphicsOptions.AaQuality = currentOption;
        }

        public void UpdateMSAAQuality()
        {
            var currentOption = (MSAAQuality)_msaaQuantity.value;
            _graphicsOptions.MsaaSampleCount = currentOption;
            _msaaAmmount.text = currentOption == MSAAQuality.None ? "NINGUNO" : currentOption.ToString();
        }

        public void UpdateRenderScale()
        {
            _graphicsOptions.RenderScale = _renderQuantity.value / 100f;
            _renderAmmount.text = $"{_renderQuantity.value}%";
        }

        public void UpdateVSync()
        {
            _graphicsOptions.Vsync = !_graphicsOptions.Vsync;
            _vsyncText.text = BooleanToName(_graphicsOptions.Vsync);
        }

        public void UpdateFrameLimiter()
        {
            var currentOption = (TargetFrame)_limiterQuantity.value;

            _limiterAmmount.text = GetTargetFrameName(currentOption);
            _graphicsOptions.TargetFrame = currentOption;
        }

        public void UpdateBloom()
        {
            _graphicsOptions.BloomEnabled = !_graphicsOptions.BloomEnabled;
            _bloomText.text = BooleanToName(_graphicsOptions.BloomEnabled);

            if (_graphicsOptions.BloomEnabled)
            {
                _bloomIntensifyCapsule.SetActive(true);
            }
            else
            {
                _bloomIntensifyCapsule.SetActive(false);
            }
        }

        public void UpdateBloomIntensity()
        {
            _graphicsOptions.BloomIntensity = _bloomIntensifyQuantity.value / 100f;
            _bloomIntensifyAmmount.text = $"{_bloomIntensifyQuantity.value}%";
        }

        public void UpdateMotionBlur()
        {
            _graphicsOptions.MotionblurEnabled = !_graphicsOptions.MotionblurEnabled;
            _motionBlurText.text = BooleanToName(_graphicsOptions.MotionblurEnabled);

            if (_graphicsOptions.MotionblurEnabled)
            {
                _motionBlurIntensifyCapsule.SetActive(true);
            }
            else
            {
                _motionBlurIntensifyCapsule.SetActive(false);
            }
        }

        public void UpdateMotionBlurIntensity()
        {
            _graphicsOptions.MotionblurIntensity = _motionBlurIntensifyQuantity.value / 100f;
            _motionBlurIntensifyAmmount.text = $"{_motionBlurIntensifyQuantity.value}%";
        }

        public void UpdateDithering()
        {
            _graphicsOptions.Dithering = !_graphicsOptions.Dithering;
            _ditheringText.text = BooleanToName(_graphicsOptions.Dithering);
        }

        public void UpdateDepthOfField()
        {
            _graphicsOptions.DepthOfFieldEnabled = !_graphicsOptions.DepthOfFieldEnabled;
            _dofText.text = BooleanToName(_graphicsOptions.DepthOfFieldEnabled);
        }

        public void UpdateAmbientOcclusion()
        {
            _graphicsOptions.AmbientOclussion = !_graphicsOptions.AmbientOclussion;
            _aoText.text = BooleanToName(_graphicsOptions.AmbientOclussion);
        }

        private string GetCorrectGraphicOptionsName(GraphicOptionsEnum graphicOptionsEnum)
        {
            switch (graphicOptionsEnum)
            {
                case GraphicOptionsEnum.Low:
                    return "BAJO";
                case GraphicOptionsEnum.Medium:
                    return "MEDIO";
                case GraphicOptionsEnum.High:
                    return "ALTO";
                case GraphicOptionsEnum.VeryHigh:
                    return "MUY ALTO";
                case GraphicOptionsEnum.Ultra:
                    return "ULTRA";
            }

            return string.Empty;
        }

        private string GetTargetFrameName(TargetFrame targetFrame)
        {
            switch (targetFrame)
            {
                case TargetFrame.None:
                    return "ILIMITADO";
                case TargetFrame.FPS59:
                    return "59 FPS";
                case TargetFrame.FPS60:
                    return "60 FPS";
                case TargetFrame.FPS120:
                    return "120 FPS";
                case TargetFrame.FPS144:
                    return "144 FPS";
            }

            return string.Empty;
        }

        private string BooleanToName(bool boolean)
        {
            return boolean ? "ACTIVADO" : "DESACTIVADO";
        }

        public void SaveChanges()
        {
            ExternalFilesManager.UpdateGameData(LevelBase.GameOptions);
            MenuConsole.menuConsole.Message("Graficos configurados");
            gameObject.SetActive(false);

            GraphicsManager.graphicsManager.SetupGraphics();
        }
    }
}
