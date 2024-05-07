using Keru.Scripts.Engine.Master;
using Keru.Scripts.Visuals.Effects;
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
        private bool _isMenu;

        [SerializeField] private List<UniversalRenderPipelineAsset> pipelines;
        [SerializeField] private UniversalRendererData _feature;

        public void SetUp(Volume volume, Fading fading, bool isMenu)
        {
            graphicsManager = this;
           
            _mainCamera = Camera.main;
            _cameraData = _mainCamera.GetComponent<UniversalAdditionalCameraData>();
            _vp = volume.profile;
            _fading = fading;
            _isMenu = isMenu;

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
         
            _cameraData.antialiasing = (AntialiasingMode)(int)graphics.AaMode;
            _cameraData.antialiasingQuality = (AntialiasingQuality)(int)graphics.AaQuality;

            var msaaSampleCount = MSAASamples.None;

            switch(graphics.MsaaSampleCount)
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
            qualityAsset.renderScale = graphics.RenderScale;

            QualitySettings.vSyncCount = graphics.Vsync ? 1 : 0;

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

            _vp.TryGet(out _depthOfField);
            if (_depthOfField != null)
            {
                _depthOfField.active = _isMenu ? false : graphics.DepthOfFieldEnabled;
            }   
            qualityAsset.supportsHDR = true;

            _feature.rendererFeatures[0].SetActive(graphics.AmbientOclussion);
            _feature.SetDirty();
        }

        public void FadeCamera(int direction)
        {
            if (_fading != null)
            {
                _fading.BeginFade(direction);
            }
        }
    }
}