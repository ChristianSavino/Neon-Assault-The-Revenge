using Keru.Scripts.Engine;

[System.Serializable]
public class GraphicsOptions
{
    //General

    //max - 0 / high - 1 / med - 2 / low - 3
    public GraphicOptionsEnum TextureQuality { get; set; }

    //none - 0 / low - 1(512) / med - 2(1024) / high - 3(2048) / ultra - 4(4096)
    public GraphicOptionsEnum ShadowQuality { get; set; }

    //min - 60 /max - 90
    public int FieldOfView { get; set; }

    //0-None /1-fxaa /2-smaa /3-taa
    public AAMode AaMode { get; set; }
    
    //0-Low / 1-Medium / 2-High
    public GraphicOptionsEnum AaQuality { get; set; }

    //1-none / x2 / x4 / x8
    public MSAAQuality MsaaSampleCount { get; set; }

    public GraphicOptionsEnum ScreenSpaceReflections { get; set; }

    //0.75 min / 2 max
    public float RenderScale { get; set; }

    //none - 0 / every v-blank - 1 / every second v-blank -2 
    public bool Vsync { get; set; }

    //0 - 59hz / 1 - 60hz / 2 - 120hz / 3 - 144hz / 4-Ninguno
    public TargetFrame TargetFrame { get; set; }

    //Post-Processing

    public bool BloomEnabled { get; set; }
    
    public float BloomIntensity { get; set; }

    public bool MotionblurEnabled { get; set; }
    
    public float MotionblurIntensity { get; set; }
    
    public bool Dithering { get; set; }
    
    public bool VolumetricLightning { get; set; }

    public bool AmbientOclussion { get; set; }

    public GraphicsOptions()
    {
        TextureQuality = GraphicOptionsEnum.VeryHigh;
        ShadowQuality = GraphicOptionsEnum.Ultra;
        FieldOfView = 90;
        AaMode = AAMode.SMAA;
        AaQuality = GraphicOptionsEnum.Low;
        MsaaSampleCount = MSAAQuality.None;
        ScreenSpaceReflections = GraphicOptionsEnum.Ultra;
        RenderScale = 1f;
        BloomEnabled = true;
        BloomIntensity = 1f;
        MotionblurEnabled = false;
        MotionblurIntensity = 0.5f;
        VolumetricLightning = false;
        AmbientOclussion = true;
    }
}
