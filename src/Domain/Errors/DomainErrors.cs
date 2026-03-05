using FluentResults;

namespace Domain.Errors;

public static class DomainErrors
{
    public static class General
    {
        public static IError NotFound(string entity, Guid id) =>
            new Error($"{entity} with id '{id}' was not found.")
                .WithMetadata("ErrorCode", "NOT_FOUND")
                .WithMetadata("Entity", entity);

        public static IError Unauthorized() =>
            new Error("You are not authorized to perform this action.")
                .WithMetadata("ErrorCode", "UNAUTHORIZED");

        public static IError Forbidden() =>
            new Error("Access to this resource is forbidden.")
                .WithMetadata("ErrorCode", "FORBIDDEN");

        public static IError Conflict(string message) =>
            new Error(message)
                .WithMetadata("ErrorCode", "CONFLICT");

        public static IError Validation(string message) =>
            new Error(message)
                .WithMetadata("ErrorCode", "VALIDATION");
    }

    public static class User
    {
        public static IError EmailAlreadyExists(string email) =>
            new Error($"A user with email '{email}' already exists.")
                .WithMetadata("ErrorCode", "EMAIL_EXISTS");

        public static IError InvalidCredentials() =>
            new Error("Invalid email or password.")
                .WithMetadata("ErrorCode", "INVALID_CREDENTIALS");

        public static IError NotFound(Guid id) => General.NotFound("User", id);
    }

    public static class TrainingPlan
    {
        public static IError NotFound(Guid id) => General.NotFound("TrainingPlan", id);

        public static IError WeekCountMismatch(int expected, int actual) =>
            new Error($"Program has {expected} weeks but {actual} week schedules were provided.")
                .WithMetadata("ErrorCode", "WEEK_COUNT_MISMATCH");

        public static IError DayOfWeekDuplicate(int dayOfWeek) =>
            new Error($"Day of week {dayOfWeek} is defined more than once in a week template.")
                .WithMetadata("ErrorCode", "DUPLICATE_DAY");
    }

    public static class WorkoutDay
    {
        public static IError NotFound(Guid id) => General.NotFound("WorkoutDay", id);
    }

    public static class WorkoutBlock
    {
        public static IError NotFound(Guid id) => General.NotFound("WorkoutBlock", id);
        public static IError InvalidBlockType(string message) =>
            new Error(message).WithMetadata("ErrorCode", "INVALID_BLOCK_TYPE");
    }

    public static class Exercise
    {
        public static IError NotFound(Guid id) => General.NotFound("Exercise", id);

        public static IError NameAlreadyExists(string name) =>
            new Error($"An exercise named '{name}' already exists.")
                .WithMetadata("ErrorCode", "EXERCISE_NAME_EXISTS");
    }

    public static class Equipment
    {
        public static IError NotFound(Guid id) => General.NotFound("Equipment", id);
    }
}