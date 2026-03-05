namespace Domain.Enums;

public enum ProgramStructureType : byte
{
    RepeatingWeek,   // 1 template week repeated N times
    FullSchedule     // every week defined explicitly
}