using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Goal.Models
{
    public class GoalTips
    {
        [Key]
        public int Id { get; set; }

        public int GoalId { get; set; }
        public Goals Goal { get; set; }

        public int TipId { get; set; }
        public Tips Tip { get; set; }
    }
}
