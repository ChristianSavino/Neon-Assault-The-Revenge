using Keru.Scripts.Engine.Module;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

namespace Keru.Scripts.Game.Cutscene
{
    public class CutsceneControls : MonoBehaviour
    {
        [Header("Objects")]
        [SerializeField] private GameObject _reflectionProbeGameObject;
        [SerializeField] private GameObject _titleScreen;
        [SerializeField] private RawImage _changesceneEffect;
        [SerializeField] private Camera _pictureCamera;
        [SerializeField] private Animator _cutsceneAnimator;
        [SerializeField] private Button _continueButton;
        [SerializeField] private Button _skipButton;

        [Header("Cutscene")]
        [SerializeField] private List<string> _sceneToPlay;
        [SerializeField] private List<string> _titleText;
        [SerializeField] private List<string> _subTitleText;
        [SerializeField] private List<CutsceneDialog> _dialogPerScene;
        [SerializeField] private List<AudioClip> _audioClip;

        private Animator _changesceneEffectAnimator;
        private CutsceneTitleScreen _titleScreenHandler;
        private UniversalAdditionalCameraData _cameraData;
        private ReflectionProbe _reflectionProbe;
        private int _currentFrame;
        private bool _isTitleScreen;
        private string _lastAnimation = "";

        private void Start()
        {
            _skipButton.gameObject.SetActive(false);

            _changesceneEffectAnimator = _changesceneEffect.GetComponent<Animator>();
            _titleScreenHandler = _titleScreen.GetComponent<CutsceneTitleScreen>();
            _pictureCamera.enabled = false;

            CreateReflectionProbe();

            _cameraData = Camera.main.GetComponent<UniversalAdditionalCameraData>();

            AdvanceCutscene();
        }

        public void AdvanceCutscene()
        {
            _continueButton.gameObject.SetActive(false);

            if (_currentFrame < _sceneToPlay.Count)
            {

                if (!_lastAnimation.Equals(_sceneToPlay[_currentFrame]))
                {
                    ChangeCutscene();
                }

                if (_dialogPerScene[_currentFrame] == null)
                {
                    if (_titleText[_currentFrame] != string.Empty || _subTitleText[_currentFrame] != string.Empty)
                    {
                        _titleScreenHandler.EnableTitleScreen(_titleText[_currentFrame], _subTitleText[_currentFrame]);
                        _isTitleScreen = true;
                    }

                    if (_titleText[_currentFrame] == string.Empty && _subTitleText[_currentFrame] == string.Empty && _isTitleScreen == true)
                    {
                        _titleScreenHandler.DisableTitleScreen();
                        _isTitleScreen = false;
                    }

                    _currentFrame++;
                    StartCoroutine(ToggleContinueButton(1f));
                }
                else
                {
                    _dialogPerScene[_currentFrame].gameObject.SetActive(true);
                    var isDestroyed = _dialogPerScene[_currentFrame].ContinueDialog();
                    if (isDestroyed)
                    {
                        _currentFrame++;
                        AdvanceCutscene();
                    }
                    else
                    {
                        StartCoroutine(ToggleContinueButton(2f));
                    }
                }
            }
            else
            {
                StartCoroutine(EndCutscene());
            }
        }

        private void ChangeCutscene()
        {
            _lastAnimation = _sceneToPlay[_currentFrame];
            _cameraData.renderPostProcessing = false;
            _pictureCamera.enabled = true;

            var activeRenderTexture = RenderTexture.active;
            RenderTexture.active = _pictureCamera.targetTexture;

            _pictureCamera.Render();

            var image = new Texture2D(1920, 1080);
            image.ReadPixels(new Rect(0, 0, _pictureCamera.targetTexture.width, _pictureCamera.targetTexture.height), 0, 0);
            image.Apply();

            _changesceneEffect.texture = image;
            _changesceneEffect.SetNativeSize();
            _changesceneEffectAnimator.Play("ChangeScene");

            RenderTexture.active = activeRenderTexture;

            _cutsceneAnimator.Play(_lastAnimation);

            _pictureCamera.enabled = false;
            _cameraData.renderPostProcessing = true;

            StartCoroutine(UpdateRenderProbe());
        }

        private void CreateReflectionProbe()
        {
            _reflectionProbe = _reflectionProbeGameObject.AddComponent<ReflectionProbe>();
            _reflectionProbe.mode = ReflectionProbeMode.Realtime;
            _reflectionProbe.size = new Vector3(30, 30, 30);
            _reflectionProbe.boxProjection = true;
            _reflectionProbe.refreshMode = ReflectionProbeRefreshMode.ViaScripting;
            _reflectionProbe.renderDynamicObjects = true;
            _reflectionProbe.resolution = 1024;
        }

        private IEnumerator UpdateRenderProbe()
        {
            yield return new WaitForSeconds(0.1f);
            _reflectionProbe.RenderProbe();
        }

        private IEnumerator ToggleContinueButton(float timeToWait)
        {
            yield return new WaitForSeconds(timeToWait);

            _continueButton.gameObject.SetActive(true);
        }

        private IEnumerator EndCutscene()
        {
            GraphicsManager.graphicsManager.FadeCamera(1);
            yield return new WaitForSeconds(2);
        }
    }
}