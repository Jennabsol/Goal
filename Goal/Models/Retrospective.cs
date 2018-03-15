using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Goal.Models
{
    public class Retrospective
    {
        [Key]
        public int Id { get; set; }

        public DateTime DateCreated { get; set; }

        [Required]
        [Display(Name = "What went well?")]
        public string Success { get; set; }

        [Required]
        [Display(Name = "What problems did you have and how did you solve them?")]
        public string Challenges { get; set; }

        [Required]
        [Display(Name = "What is you current status in relation to your goal?")]
        public string EndState { get; set; }

        public int GoalId { get; set; }
        public Goals Goal { get; set; }
    }
}
