using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiftBro.Model
{
    public class UserProgram
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }
        public Program Program { get; set; }
        public User User { get; set; }
        public bool CurrentlyUsing { get; set; }
    }
}