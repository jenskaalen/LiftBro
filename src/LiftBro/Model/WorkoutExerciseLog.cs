using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftBro.Model
{
    public class WorkoutExerciseLog
    {
        public Guid Id { get; set; }
        public WorkoutExercise WorkoutExercise { get; set; }
        public User User { get; set; }
        public DateTime When { get; set; }
        public string Message { get; set; }
    }
}
