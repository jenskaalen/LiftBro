using System.Data.Entity.ModelConfiguration.Conventions;
using LiftBro.Model;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftBro
{
    public class LiftBroContext: DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Exercise> Exercises { get; set; }
        public virtual DbSet<Program> Programs { get; set; }
        public virtual DbSet<WorkoutExercise> WorkoutExercises { get; set; }
        public virtual DbSet<WorkoutDay> WorkoutDays { get; set; }
        //public virtual DbSet<WorkoutWeek> WorkoutWeeks { get; set; }
        public virtual DbSet<Set> Sets { get; set; }
        public virtual DbSet<UserExercise> UserExercises { get; set; }
        public virtual DbSet<UserProgram> UserPrograms { get; set; }
        public virtual DbSet<CompletedWorkoutDay> CompletedWorkouts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WorkoutExercise>()
                .HasMany(w => w.Sets)
                .WithOptional()
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<WorkoutDay>()
                .HasMany(w => w.Exercises)
                .WithOptional()
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Program>()
                .HasMany(w => w.WorkoutDays)
                .WithOptional()
                .WillCascadeOnDelete(true);



            //modelBuilder.Entity<UserProgram>()
            //    .HasRequired(w => w.Program)
            //    .
            //    .WillCascadeOnDelete(true);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}

