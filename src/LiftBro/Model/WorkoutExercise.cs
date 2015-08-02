using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiftBro.Model
{
    public class WorkoutExercise
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }
        public List<Set> Sets { get; set; }
        public Exercise Exercise { get; set; }
        public int Order { get; set; }
        public bool CurrentExercise { get; set; }
    }
}