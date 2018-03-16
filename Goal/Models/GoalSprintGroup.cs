using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Goal.Models
{
    public class GoalSprintGroup
    {
        [Key]
        public int Id { get; set; }

        public int GoalsId { get; set; }
        public Goals Goal { get; set; }

        public int SprintGroupId { get; set; }
        public SprintGroup SprintGroup { get; set; }
    }
}
