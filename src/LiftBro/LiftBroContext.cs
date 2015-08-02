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
        public DbSet<User> Users { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Program> Programs { get; set; }
        public DbSet<WorkoutExercise> WorkoutExercises { get; set; }
        public DbSet<WorkoutDay> WorkoutDays { get; set; }
        //public DbSet<WorkoutWeek> WorkoutWeeks { get; set; }
        public DbSet<Set> Sets { get; set; }
        public DbSet<UserExercise> UserExercises { get; set; }
        public DbSet<UserProgram> UserPrograms { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}

