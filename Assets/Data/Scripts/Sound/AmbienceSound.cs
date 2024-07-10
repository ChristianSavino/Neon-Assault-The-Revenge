using Keru.Scripts.Engine;
using Keru.Scripts.Engine.Module;
using UnityEngine;

public class AmbienceSound : MonoBehaviour
{
    [SerializeField] private bool _loops;
    [SerializeField] private AudioClip _clipToReproduce;
    [SerializeField] private SoundType _sourceType;
    [SerializeField] private bool _is3dAudio;
    [SerializeField] private float _maxDistance;
    [SerializeField] private float _minDistance;
    [SerializeField] private AudioRolloffMode _rollOffMode;

    private void Start()
    {
        var audioSource = AudioManager.audioManager.CreateNewAudioSource(gameObject, _sourceType);
        audioSource.clip = _clipToReproduce;
        audioSource.loop = _loops;
        if(_is3dAudio)
        {
            audioSource.spatialBlend = 1;
            audioSource.maxDistance = _maxDistance;
            audioSource.minDistance = _minDistance;
            audioSource.rolloffMode = _rollOffMode;
        }

        audioSource.Play();
    }
}
