using Keru.Scripts.Engine.Master;
using Keru.Scripts.Visuals.Effects;
using LimWorks.Rendering.URP.ScreenSpaceReflections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Keru.Scripts.Engine.Module
{
    public class GraphicsManager : MonoBehaviour
    {
        public static GraphicsManager graphicsManager;
        private static Fading _fading;

        private Camera _mainCamera;
        private UniversalAdditionalCameraData _cameraData;
       
        private VolumeProfile _vp;
        private DepthOfField _depthOfField;
        private Bloom _bloom;
        private MotionBlur _motionBlur;
        private VolumetricFogVolumeComponent _volumetric;
        private bool _isMenu;

        [SerializeField] private List<UniversalRenderPipelineAsset> pipelines;
        [SerializeField] private UniversalRendererData _feature;

        private int _ambientOclussionFeature = 0;
        private int _ssrFeature = 4;
        private int _hiRezSsrFeature = 5;
        private bool _useMainVolumetricLights;
        private bool _useExtraVolumetricLights;

        public void SetUp(Volume volume, Fading fading, bool isMenu, bool useMainVolumetric, bool useExtraVolumetricLights)
        {
            graphicsManager = this;
           
            _mainCamera = Camera.main;
            _cameraData = _mainCamera.GetComponent<UniversalAdditionalCameraData>();
            _vp = volume.profile;
            _fading = fading;
            _isMenu = isMenu;
            _useMainVolumetricLights = useMainVolumetric;
            _useExtraVolumetricLights = useExtraVolumetricLights;

            SetupGraphics();
        }

        public void SetupGraphics()
        {
            _mainCamera.backgroundColor = Color.black;
            _mainCamera.nearClipPlane = 0.1f;
            _cameraData.renderShadows = true;
            _cameraData.renderPostProcessing = true;

            var graphics = LevelBase.GameOptions.Options.GraphicsOptions;
            QualitySettings.globalTextureMipmapLimit = 3-(int)graphics.TextureQuality;
            
            QualitySettings.renderPipeline = pipelines[(int)graphics.ShadowQuality];
            var qualityAsset = (UniversalRenderPipelineAsset)QualitySettings.renderPipeline;

            if(!_isMenu)
            {
                _mainCamera.fieldOfView = graphics.FieldOfView;
            }

            SetMsaa(graphics, qualityAsset);

            qualityAsset.renderScale = graphics.RenderScale;

            QualitySettings.vSyncCount = graphics.Vsync ? 1 : 0;

            SetTargetFrame(graphics);

            _vp.TryGet(out _bloom);
            if (_bloom != null)
            {
                _bloom.active = graphics.BloomEnabled;
                _bloom.intensity.Override(graphics.BloomIntensity);
            }

            _vp.TryGet(out _motionBlur);
            if (_motionBlur != null)
            {
                _motionBlur.active = graphics.MotionblurEnabled;
                _motionBlur.intensity.Override(graphics.MotionblurIntensity);
            }

            _cameraData.dithering = graphics.Dithering;

            qualityAsset.supportsHDR = true;

            _feature.rendererFeatures[_ambientOclussionFeature].SetActive(graphics.AmbientOclussion);

            var toggleMainVolumetricLights = graphics.VolumetricLightning && _useMainVolumetricLights;
            var toggleExtraVolumetricLights = graphics.VolumetricLightning && _useExtraVolumetricLights;

            _vp.TryGet(out _volumetric);
            if (_volumetric != null)
            {
                _volumetric.active = toggleMainVolumetricLights || toggleExtraVolumetricLights;
                _volumetric.enableMainLightContribution.Override(toggleMainVolumetricLights);
                _volumetric.enableAdditionalLightsContribution.Override(toggleExtraVolumetricLights);
            }

            SetScreenSpaceReflections(graphics);

            _feature.SetDirty();
        }

        public void FadeCamera(int direction)
        {
            if (_fading != null)
            {
                _fading.BeginFade(direction);
            }
        }

        private void SetScreenSpaceReflections(GraphicsOptions graphics)
        {
            if(graphics.ScreenSpaceReflections != GraphicOptionsEnum.Low)
            {
                _feature.rendererFeatures[_ssrFeature].SetActive(true);
                
                var settings = new ScreenSpaceReflectionsSettings()
                {
                    Downsample = (int)graphics.ScreenSpaceReflections > (int)GraphicOptionsEnum.High ? (uint)0 : (uint)1,
                    MinSmoothness = 0.5f,
                    DitherMode = DitherMode.InterleavedGradient,
                    MaxSteps = 32 * (int)graphics.ScreenSpaceReflections,
                    StepStrideLength = 0.03f,
                    TracingMode = RaytraceModes.LinearTracing
                };

                if(graphics.ScreenSpaceReflections == GraphicOptionsEnum.Ultra)
                {
                    settings.TracingMode = RaytraceModes.HiZTracing;
                    _feature.rendererFeatures[_hiRezSsrFeature].SetActive(true);
                }
                else
                {
                    _feature.rendererFeatures[_hiRezSsrFeature].SetActive(false);
                }

                LimSSR.SetSettings(settings);
            }
            else
            {
                _feature.rendererFeatures[_ssrFeature].SetActive(false);
                _feature.rendererFeatures[_hiRezSsrFeature].SetActive(false);
            }
        }

        private void SetMsaa(GraphicsOptions graphics, UniversalRenderPipelineAsset qualityAsset)
        {
            _cameraData.antialiasing = (AntialiasingMode)(int)graphics.AaMode;
            _cameraData.antialiasingQuality = (AntialiasingQuality)(int)graphics.AaQuality;

            var msaaSampleCount = MSAASamples.None;

            switch (graphics.MsaaSampleCount)
            {
                case MSAAQuality.X2:
                    msaaSampleCount = MSAASamples.MSAA2x;
                    break;
                case MSAAQuality.X4:
                    msaaSampleCount = MSAASamples.MSAA4x;
                    break;
                case MSAAQuality.X8:
                    msaaSampleCount = MSAASamples.MSAA8x;
                    break;
            }

            qualityAsset.msaaSampleCount = (int)msaaSampleCount;
        }

        private void SetTargetFrame(GraphicsOptions graphics)
        {
            var targetFrame = -1;
            switch (graphics.TargetFrame)
            {
                case TargetFrame.FPS59:
                    targetFrame = 59;
                    break;
                case TargetFrame.FPS60:
                    targetFrame = 60;
                    break;
                case TargetFrame.FPS120:
                    targetFrame = 120;
                    break;
                case TargetFrame.FPS144:
                    targetFrame = 144;
                    break;
            }

            Application.targetFrameRate = targetFrame;
        }
    }
}