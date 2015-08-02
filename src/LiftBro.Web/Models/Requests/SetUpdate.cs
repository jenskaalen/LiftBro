using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiftBro.Model;

namespace LiftBro.Web.Models.Requests
{
    public class SetUpdate
    {
        public WorkoutExercise Exercise { get; set; }
        public Set Set { get; set; }
        public ChangeModifier Modifier { get; set; }
    }
}
