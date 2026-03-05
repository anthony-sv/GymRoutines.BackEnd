using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Equipment> Equipment => Set<Equipment>();
    public DbSet<EquipmentVariant> EquipmentVariants => Set<EquipmentVariant>();
    public DbSet<Exercise> Exercises => Set<Exercise>();
    public DbSet<TrainingPlan> TrainingPlans => Set<TrainingPlan>();
    public DbSet<WeekTemplate> WeekTemplates => Set<WeekTemplate>();
    public DbSet<WorkoutDay> WorkoutDays => Set<WorkoutDay>();
    public DbSet<WorkoutBlock> WorkoutBlocks => Set<WorkoutBlock>();
    public DbSet<StandardSetBlock> StandardSetBlocks => Set<StandardSetBlock>();
    public DbSet<CircuitBlock> CircuitBlocks => Set<CircuitBlock>();
    public DbSet<CircuitExercise> CircuitExercises => Set<CircuitExercise>();
    public DbSet<CardioBlock> CardioBlocks => Set<CardioBlock>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}