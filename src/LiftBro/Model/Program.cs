using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftBro.Model
{
    public class Program
    {
        public string Name { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }
        public List<WorkoutDay> WorkoutDays { get; set; }
        public User Creator { get; set; }
    }
}
