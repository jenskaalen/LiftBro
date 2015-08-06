using LiftBro.Model;

namespace LiftBro.Web.Models.Requests
{
    public class WorkoutDayExerciseChange
    {
        public WorkoutDay WorkoutDay { get; set; }
        public Exercise Exercise { get; set; }
        public ChangeModifier Modifier { get; set; } 
    }
}