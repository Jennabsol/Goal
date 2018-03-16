using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Goal.Models
{
    public class SprintGroup
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public DateTime DateCreated { get; set; }

        public bool Completed { get; set; }

        public SprintGroup()
        {
            Completed = false;
        }

        public virtual ICollection<GoalSprintGroup> GoalSprintGroup { get; set; }
    }
}
