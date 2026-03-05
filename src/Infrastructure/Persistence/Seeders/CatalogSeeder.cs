using Domain.Enums;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Seeders;

public sealed class CatalogSeeder(AppDbContext db, ILogger<CatalogSeeder> logger)
{
    public async Task SeedAsync(CancellationToken ct = default)
    {
        await SeedEquipmentAsync(ct);
        await SeedExercisesAsync(ct);
        logger.LogInformation("Catalog seeding complete.");
    }

    private async Task SeedEquipmentAsync(CancellationToken ct)
    {
        if (await db.Equipment.AnyAsync(e => e.IsSeeded, ct)) return;

        var barbell = Equipment.Create("Barbell", EquipmentCategory.Barbell, "Standard Olympic barbell", isSeeded: true);
        barbell.AddVariant("Standard", "45 lb Olympic bar");
        barbell.AddVariant("EZ Curl Bar", "Angled curl bar for reduced wrist strain");
        barbell.AddVariant("Trap/Hex Bar", "Hexagonal bar for deadlifts");

        var dumbbell = Equipment.Create("Dumbbell", EquipmentCategory.Dumbbell, "Free weight dumbbell", isSeeded: true);
        dumbbell.AddVariant("Fixed Weight", "Standard fixed dumbbell");
        dumbbell.AddVariant("Adjustable", "Adjustable dumbbell");

        var cable = Equipment.Create("Cable Machine", EquipmentCategory.Cable, "Cable pulley machine", isSeeded: true);
        cable.AddVariant("Rope", "Triceps/hammer curl rope attachment");
        cable.AddVariant("Straight Bar", "Straight bar attachment");
        cable.AddVariant("V-Bar", "V-shaped bar attachment");
        cable.AddVariant("D-Handle", "Single D-handle");
        cable.AddVariant("Lat Bar", "Wide lat pulldown bar");
        cable.AddVariant("EZ Attachment", "EZ curl cable attachment");

        var machine = Equipment.Create("Machine", EquipmentCategory.Machine, "Weight-stack machine", isSeeded: true);
        machine.AddVariant("Dip Machine / Tricep Press", "Seated machine dip");
        machine.AddVariant("Preacher Curl Machine", "Machine preacher curl");
        machine.AddVariant("Leg Press", "Seated leg press");
        machine.AddVariant("Chest Press", "Seated machine chest press");

        var cardioMachine = Equipment.Create("Cardio Machine", EquipmentCategory.CardioMachine, "Cardio equipment", isSeeded: true);
        cardioMachine.AddVariant("Treadmill", "Motor-driven treadmill");
        cardioMachine.AddVariant("Stairmaster / StepMill", "Revolving stair climber");
        cardioMachine.AddVariant("Elliptical", "Elliptical cross-trainer");
        cardioMachine.AddVariant("Stationary Bike", "Upright stationary bike");
        cardioMachine.AddVariant("Rowing Machine", "Cable/flywheel rower");

        var bodyweight = Equipment.Create("Bodyweight", EquipmentCategory.Bodyweight, "No equipment needed", isSeeded: true);
        bodyweight.AddVariant("None", "No equipment");
        bodyweight.AddVariant("Dip Bars", "Parallel dip bars");
        bodyweight.AddVariant("Pull-Up Bar", "Overhead bar");

        db.Equipment.AddRange(barbell, dumbbell, cable, machine, cardioMachine, bodyweight);
        await db.SaveChangesAsync(ct);
        logger.LogInformation("Equipment seeded.");
    }

    private async Task SeedExercisesAsync(CancellationToken ct)
    {
        if (await db.Exercises.AnyAsync(e => e.IsSeeded, ct)) return;

        var barbellId = (await db.Equipment.FirstAsync(e => e.Name == "Barbell" && e.IsSeeded, ct)).Id;
        var dumbbellId = (await db.Equipment.FirstAsync(e => e.Name == "Dumbbell" && e.IsSeeded, ct)).Id;
        var cableId = (await db.Equipment.FirstAsync(e => e.Name == "Cable Machine" && e.IsSeeded, ct)).Id;
        var machineId = (await db.Equipment.FirstAsync(e => e.Name == "Machine" && e.IsSeeded, ct)).Id;

        var ropeVariantId = (await db.EquipmentVariants.FirstAsync(v => v.Name == "Rope", ct)).Id;
        var machineDipVariantId = (await db.EquipmentVariants.FirstAsync(v => v.Name == "Dip Machine / Tricep Press", ct)).Id;
        var ezBarVariantId = (await db.EquipmentVariants.FirstAsync(v => v.Name == "EZ Curl Bar", ct)).Id;

        var exercises = new List<Exercise>
        {
            // BICEPS - Strength
            Exercise.Create("Standing Barbell Curl", ExerciseType.Strength,
                "Classic standing barbell biceps curl",
                barbellId, null,
                [MuscleGroup.Biceps], [MuscleGroup.Forearms],
                isSeeded: true),

            Exercise.Create("Standing EZ Bar Curl", ExerciseType.Strength,
                "EZ bar curl - reduces wrist supination stress",
                barbellId, ezBarVariantId,
                [MuscleGroup.Biceps], [MuscleGroup.Forearms],
                isSeeded: true),

            Exercise.Create("Dumbbell Hammer Curl", ExerciseType.Strength,
                "Neutral grip curl targeting brachialis and brachioradialis",
                dumbbellId, null,
                [MuscleGroup.Biceps, MuscleGroup.Forearms], [],
                isSeeded: true),

            Exercise.Create("Incline Dumbbell Curl", ExerciseType.Strength,
                "Performed on incline bench for full bicep stretch",
                dumbbellId, null,
                [MuscleGroup.Biceps], [],
                isSeeded: true),

            Exercise.Create("Standing Incline Cable Curl", ExerciseType.Strength,
                "Cable curl with cable at high incline for constant tension",
                cableId, null,
                [MuscleGroup.Biceps], [],
                isSeeded: true),

            Exercise.Create("Cable Preacher Curl", ExerciseType.Strength,
                "Preacher curl using low cable pulley",
                cableId, null,
                [MuscleGroup.Biceps], [],
                isSeeded: true),

            Exercise.Create("Concentration Curl", ExerciseType.Strength,
                "Seated dumbbell concentration curl",
                dumbbellId, null,
                [MuscleGroup.Biceps], [],
                isSeeded: true),

            // BICEPS - Timed/Pose
            Exercise.Create("Front Double Biceps Flex", ExerciseType.Timed,
                "Classic front double biceps bodybuilding pose hold",
                null, null,
                [MuscleGroup.Biceps], [MuscleGroup.Shoulders],
                isSeeded: true),

            Exercise.Create("Biceps Squeeze Contraction Hold", ExerciseType.Timed,
                "Isometric peak contraction hold for mind-muscle connection",
                null, null,
                [MuscleGroup.Biceps], [],
                isSeeded: true),

            // TRICEPS - Strength
            Exercise.Create("Machine Dip", ExerciseType.Strength,
                "Seated machine dip isolating triceps",
                machineId, machineDipVariantId,
                [MuscleGroup.Triceps], [MuscleGroup.Chest, MuscleGroup.Shoulders],
                isSeeded: true),

            Exercise.Create("EZ Bar Skull Crusher", ExerciseType.Strength,
                "Lying triceps extension with EZ bar",
                barbellId, ezBarVariantId,
                [MuscleGroup.Triceps], [],
                isSeeded: true),

            Exercise.Create("Cable Triceps Pushdown with Rope", ExerciseType.Strength,
                "Cable pushdown using rope attachment, flaring out at bottom",
                cableId, ropeVariantId,
                [MuscleGroup.Triceps], [],
                isSeeded: true),

            Exercise.Create("Overhead Cable Triceps Extension", ExerciseType.Strength,
                "Overhead extension using cable for constant tension",
                cableId, null,
                [MuscleGroup.Triceps], [],
                isSeeded: true),

            Exercise.Create("Close-Grip Bench Press", ExerciseType.Strength,
                "Bench press with close grip targeting triceps",
                barbellId, null,
                [MuscleGroup.Triceps], [MuscleGroup.Chest, MuscleGroup.Shoulders],
                isSeeded: true),

            Exercise.Create("Dumbbell Kickback", ExerciseType.Strength,
                "Bent-over dumbbell triceps kickback",
                dumbbellId, null,
                [MuscleGroup.Triceps], [],
                isSeeded: true),

            // CHEST
            Exercise.Create("Flat Barbell Bench Press", ExerciseType.Strength,
                "Compound push movement for overall chest",
                barbellId, null,
                [MuscleGroup.Chest], [MuscleGroup.Triceps, MuscleGroup.Shoulders],
                isSeeded: true),

            Exercise.Create("Incline Dumbbell Press", ExerciseType.Strength,
                "Upper chest focus with incline dumbbell",
                dumbbellId, null,
                [MuscleGroup.Chest], [MuscleGroup.Triceps, MuscleGroup.Shoulders],
                isSeeded: true),

            // BACK
            Exercise.Create("Lat Pulldown", ExerciseType.Strength,
                "Cable lat pulldown for lat width",
                cableId, null,
                [MuscleGroup.Back], [MuscleGroup.Biceps],
                isSeeded: true),

            Exercise.Create("Seated Cable Row", ExerciseType.Strength,
                "Cable rowing for mid-back thickness",
                cableId, null,
                [MuscleGroup.Back], [MuscleGroup.Biceps],
                isSeeded: true),

            // LEGS
            Exercise.Create("Barbell Squat", ExerciseType.Strength,
                "King of leg exercises - compound squat",
                barbellId, null,
                [MuscleGroup.Quadriceps, MuscleGroup.Glutes], [MuscleGroup.Hamstrings, MuscleGroup.Core],
                isSeeded: true),

            Exercise.Create("Romanian Deadlift", ExerciseType.Strength,
                "Hip hinge for hamstrings and glutes",
                barbellId, null,
                [MuscleGroup.Hamstrings, MuscleGroup.Glutes], [MuscleGroup.Back],
                isSeeded: true),

            // CARDIO
            Exercise.Create("Treadmill Walk", ExerciseType.Cardio,
                "Indoor walking on treadmill",
                null, null, [], [],
                defaultCardioType: CardioType.Walking,
                isSeeded: true),

            Exercise.Create("Stair Climbing", ExerciseType.Cardio,
                "Stairmaster or stepmill machine",
                null, null, [], [],
                defaultCardioType: CardioType.StairClimbing,
                isSeeded: true),

            Exercise.Create("Stationary Bike", ExerciseType.Cardio,
                "Upright stationary cycling",
                null, null, [], [],
                defaultCardioType: CardioType.Cycling,
                isSeeded: true),
        };

        db.Exercises.AddRange(exercises);
        await db.SaveChangesAsync(ct);
        logger.LogInformation("Exercises seeded ({Count} exercises).", exercises.Count);
    }
}