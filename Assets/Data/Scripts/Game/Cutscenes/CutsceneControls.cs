using Keru.Scripts.Engine.Master;
using Keru.Scripts.Engine.Module;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        [SerializeField] private CutsceneDialog _cutsceneDialog;

        [Header("Cutscene")]
        [SerializeField] private List<CutsceneScene> _scenesToPlay;

        private Animator _changesceneEffectAnimator;
        private CutsceneTitleScreen _titleScreenHandler;
        private UniversalAdditionalCameraData _cameraData;
        private ReflectionProbe _reflectionProbe;
        private AudioSource _audioSource;
        private int _currentFrame;
        private int _currentDialogFrame;
        private string _lastAnimation = string.Empty;
        private bool _isBlackScreen;

        private void Start()
        {
            _skipButton.gameObject.SetActive(false);
            StartCoroutine(ToggleSkipButton());

            _changesceneEffectAnimator = _changesceneEffect.GetComponent<Animator>();
            _titleScreenHandler = _titleScreen.GetComponent<CutsceneTitleScreen>();
            _pictureCamera.enabled = false;

            _audioSource = AudioManager.audioManager.CreateNewAudioSource(gameObject, Engine.SoundType.Effect);

            CreateReflectionProbe();

            _cameraData = Camera.main.GetComponent<UniversalAdditionalCameraData>();

            AdvanceCutscene();
        }

        public void SkipCutscene()
        {
            CompleteLevel();
        }

        public void AdvanceCutscene()
        {
            _continueButton.gameObject.SetActive(false);

            if (_currentFrame < _scenesToPlay.Count)
            {
                var currentFrameData = _scenesToPlay[_currentFrame];
                var timeToRestartButton = 1f;

                if (currentFrameData.SoundToPlay != null)
                {
                    _audioSource.PlayOneShot(currentFrameData.SoundToPlay);
                }

                if(_lastAnimation != currentFrameData.AnimationName)
                {
                    ChangeCutscene(currentFrameData.AnimationName);
                }

                if (currentFrameData.BlackBoxText != string.Empty || currentFrameData.BlackBoxDescription != string.Empty && !_isBlackScreen)
                {
                    timeToRestartButton = 0;
                    _titleScreenHandler.EnableTitleScreen(currentFrameData.BlackBoxText, currentFrameData.BlackBoxDescription, _continueButton.gameObject);
                    _isBlackScreen = true;
                }

                if (currentFrameData.BlackBoxText == string.Empty && currentFrameData.BlackBoxDescription == string.Empty && _isBlackScreen)
                {
                    timeToRestartButton = 0;
                    if (currentFrameData.CharacterDialog.Count != 0)
                    {
                        _titleScreenHandler.DisableTitleScreen();
                    }
                    else
                    {
                        _titleScreenHandler.DisableTitleScreen(_continueButton.gameObject);
                    }
                    _isBlackScreen = false;

                }

                if(_currentDialogFrame < currentFrameData.CharacterDialog.Count)
                {
                    timeToRestartButton = 0;
                    _cutsceneDialog.gameObject.SetActive(true);                    
                    _cutsceneDialog.LoadDialog(currentFrameData.CharacterNameDialog[_currentDialogFrame], currentFrameData.CharacterDialog[_currentDialogFrame], _continueButton.gameObject);             
                    _currentDialogFrame++;
                    
                    if (_currentDialogFrame >= currentFrameData.CharacterDialog.Count)
                    {
                        _currentDialogFrame = 0;
                        _currentFrame++;
                    }    
                }
                else
                {
                    _cutsceneDialog.gameObject.SetActive(false);
                    _currentDialogFrame = 0;
                    _currentFrame++;
                }

                if(timeToRestartButton > 0)
                {
                    StartCoroutine(ToggleContinueButton(timeToRestartButton));
                }               
            }
            else
            {
                CompleteLevel();                
            }
        }

        private void ChangeCutscene(string animation)
        {
            _lastAnimation = animation;
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

        private IEnumerator ToggleSkipButton()
        {
            yield return new WaitForSeconds(5);
            _skipButton.gameObject.SetActive(true);
        }
        private void CompleteLevel()
        {
            LevelBase.levelBase.CompleteLevel();
        }
    }
}

[Serializable]
public class CutsceneScene
{
    public string AnimationName;
    public AudioClip SoundToPlay;
    public List<string> CharacterNameDialog;
    public List<string> CharacterDialog;
    public string BlackBoxText;
    public string BlackBoxDescription;
}