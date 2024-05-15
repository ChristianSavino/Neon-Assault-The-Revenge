using Keru.Scripts.Engine;
using Keru.Scripts.Engine.Module;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceSound : MonoBehaviour
{
    [SerializeField] private bool _loops;
    [SerializeField] private AudioClip _clipToReproduce;
    [SerializeField] private SoundType _sourceType;

    private void Start()
    {
        var audioSource = AudioManager.audioManager.CreateNewAudioSource(gameObject, _sourceType);
        audioSource.clip = _clipToReproduce;
        audioSource.loop = _loops;

        audioSource.Play();
    }
}
