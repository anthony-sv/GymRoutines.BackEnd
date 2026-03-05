namespace Domain.Enums;

public enum IntensityTechnique : byte
{
    None,
    DropSet,      // reduce weight each drop
    RestPause,    // mini-rest within the set
    TUT           // time under tension (tempo)
}