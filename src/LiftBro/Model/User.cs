using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiftBro.Model
{
    public class User
    {
        public string Name { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        //public Guid Id { get; set; }
        [Key]
        public string Username { get; set; }
    }
}