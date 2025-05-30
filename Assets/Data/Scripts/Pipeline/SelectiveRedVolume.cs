using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

// Define el parámetro editable en el volumen
[System.Serializable]
public class ClampedFloatParameter : VolumeParameter<float>
{
    public ClampedFloatParameter(float value, float min, float max, bool overrideState = false)
        : base(value, overrideState)
    {
        this.min = min;
        this.max = max;
    }

    public float min;
    public float max;

    public override void Interp(float from, float to, float t)
    {
        value = Mathf.Lerp(from, to, t);
        value = Mathf.Clamp(value, min, max);
    }
}

[VolumeComponentMenu("Custom/Selective Red")]
public class SelectiveRedVolume : VolumeComponent, IPostProcessComponent
{
    // Parámetro intensidad controlable desde el volumen
    public ClampedFloatParameter intensity = new ClampedFloatParameter(0f, 0f, 1f);

    public bool IsActive() => intensity.value > 0f;

    public bool IsTileCompatible() => true;
}