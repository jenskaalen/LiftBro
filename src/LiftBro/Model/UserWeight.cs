using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiftBro.Model
{
    public class UserWeight
    {
        public Guid Id { get; set; }
        public User User { get; set; }
        public double Weight { get; set; }
        public DateTime When { get; set; }
    }
}
