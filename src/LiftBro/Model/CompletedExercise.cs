using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftBro.Model
{
    public class CompletedExercise
    {
        public WorkoutExercise Exercise { get; set; }
        public User User { get; set; }
        public DateTime When { get; set; }
    }
}
