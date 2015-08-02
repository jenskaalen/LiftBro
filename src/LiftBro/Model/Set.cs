using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiftBro.Model
{
    public class Set
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }
        /// <summary>
        /// Percentage of one rep max
        /// </summary>
        public double ORMPercentage { get; set; }
        public int Reps { get; set; }
        public int Order { get; set; }
    }
}