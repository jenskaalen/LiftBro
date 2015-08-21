using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftBro.Model
{
    public class UserProgramExercise
    {
        //TODO: possibly remove, this might be redundant
        [Key, Column(Order = 0)]
        public User User { get; set; }
        [Key, Column(Order = 1)]
        public Program Program { get; set; }
        [Key, Column(Order = 2)]
        public WorkoutDay WorkoutDay { get; set; }
        public double WeightAdded { get; set; }
    }
}
