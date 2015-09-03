using LiftBro.Model;

namespace LiftBro.Web.Models.Requests
{
    public class WorkoutDayChange
    {
        public Program Program { get; set; }
        public WorkoutDay WorkoutDay { get; set; }
        public ChangeModifier Modifier { get; set; }
    }
}