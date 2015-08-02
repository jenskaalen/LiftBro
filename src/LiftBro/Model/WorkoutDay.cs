using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiftBro.Model
{
    public class WorkoutDay
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }
        public List<WorkoutExercise> Exercises { get; set; }
        public int Order { get; set; }
    }
}