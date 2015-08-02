using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftBro.Model
{
    public class ProgramExercise
    {
        public Program Program { get; set; }
        public Exercise Exercise { get; set; }
        /// <summary>
        /// Weight increase interval in workout days
        /// </summary>
        public int WeightIncreaseDaysInterval { get; set; }
        public int WeightIncrease { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
