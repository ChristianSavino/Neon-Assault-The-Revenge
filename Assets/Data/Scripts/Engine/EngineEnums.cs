namespace Keru.Scripts.Engine
{
    public enum Difficulty
    {
        Easy = 0,
        Normal = 1,
        Hard = 2,
        KeruMustDie = 3,
    }

    public enum SoundType
    {
        Effect = 0,
        Music = 1,
        Voice = 2
    }

    public enum MusicTracks
    {
        Ambience = 0,
        Assault = 1,
        Boss = 2
    }

    public enum GraphicOptionsEnum
    {
        Low = 0,
        Medium = 1,
        High = 2,
        VeryHigh = 3,
        Ultra = 4
    }

    public enum AAMode
    {
        None = 0,
        FXAA = 1,
        SMAA = 2,
        TAA = 3
    }

    public enum MSAAQuality
    {
        None = 0,
        X2 = 1,
        X4 = 2,
        X8 = 3
    }

    public enum TargetFrame
    {
        None = 0,
        FPS59 = 1,
        FPS60 = 2,
        FPS120 = 3,
        FPS144 = 4
    }
}