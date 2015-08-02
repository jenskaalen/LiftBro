using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiftBro.Model
{
    public class Exercise
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}