using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Models
{
    public class Trail
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Distance { get; set; }        
        public DifficultyType Difficulty { get; set; }
        public virtual NationalPark NationalPark { get; set; }
        public DateTime DateCreated { get; set;  }
    }
    public enum DifficultyType { Easy, Moderate, Difficulty, Expert }
}
