using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftBro.Model
{
    public class UserExercise
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }
        public double OneRepetationMax { get; set; }
        public User User { get; set; }
        public Exercise Exercise { get; set; }
    }
}
