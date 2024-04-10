[System.Serializable]
public class AudioOptions
{
    public float MusicVolume { get; set; }
    public float EffectVolume { get; set; }
    public float VoiceVolume { get; set; }
    public int Surround { get; set; }

    public AudioOptions()
    {
        MusicVolume = 0.5f;
        EffectVolume = 0.4f;
        VoiceVolume = 0.6f;
        Surround = 2;
    }
}
