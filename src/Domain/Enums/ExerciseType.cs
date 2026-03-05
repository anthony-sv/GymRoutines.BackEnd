namespace Domain.Enums;

public enum ExerciseType : byte
{
    Strength,     // rep-based
    Timed,        // duration-based (poses, planks, holds, flexes)
    Cardio        // cardio-specific
}