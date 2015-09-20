using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiftBro.Model;

namespace LiftBro.Web.Models.Requests
{
    public class CreateSet
    {
        public WorkoutExercise Exercise { get; set; }
        public Set Set { get; set; }
    }

    public class CreateWorkoutExercise
    {
        public WorkoutDay WorkoutDay { get; set; }
        public WorkoutExercise Exercise { get; set; }
    }

    public class CreateWorkoutDay
    {
        public Program Program { get; set; }
        public WorkoutDay WorkoutDay { get; set; }
    }
}
