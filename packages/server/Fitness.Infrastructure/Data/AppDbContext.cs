
using Fitness.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace Fitness.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<ExerciseLog> ExerciseLogs { get; set; }
        public DbSet<SetLog> SetLogs { get; set; }
        public DbSet<WorkoutSession> WorkoutSessions { get; set; }
        public DbSet<WorkoutTemplate> WorkoutTemplates { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorkoutTemplate>()
                .HasOne(wt => wt.User)
                .WithMany(u => u.WorkoutTemplates)
                .HasForeignKey(wt => wt.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WorkoutTemplate>()
               .HasMany(wt => wt.Exercises)
               .WithMany(e => e.WorkoutTemplates)
               .UsingEntity(j => j.ToTable("WorkoutTemplateExercises"));

            modelBuilder.Entity<WorkoutSession>()
                .HasOne(ws => ws.User)
                .WithMany(u => u.WorkoutSessions)
                .HasForeignKey(ws => ws.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WorkoutSession>()
                .HasOne(ws => ws.WorkoutTemplate)
                .WithMany(wt => wt.WorkoutSessions)
                .HasForeignKey(ws => ws.WorkoutTemplateId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ExerciseLog>()
                .HasOne(el => el.WorkoutSession)
                .WithMany(ws => ws.ExerciseLogs)
                .HasForeignKey(el => el.WorkoutSessionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ExerciseLog>()
                .HasOne(el => el.Exercise)
                .WithMany(e => e.ExerciseLogs)
                .HasForeignKey(el => el.ExerciseId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SetLog>()
                .HasOne(sl => sl.ExerciseLog)
                .WithMany(el => el.SetLogs)
                .HasForeignKey(sl => sl.ExerciseLogId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WorkoutSession>()
                .Property(e => e.Status)
                .HasConversion<string>();

            modelBuilder.Entity<Exercise>()
                .Property(e => e.MuscleGroup)
                .HasConversion<string>();
        }
    }
}
