using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftBro.Model
{
    public class CompletedWorkoutDay
    {
        [Key, Column(Order = 0)]
        public WorkoutDay Workout { get; set; }
        [Key, Column(Order = 1)]
        public User User { get; set; }
        [Key, Column(Order = 2)]
        public DateTime When { get; set; }
    }
}
