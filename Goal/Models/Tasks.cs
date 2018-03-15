using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Goal.Models
{
    public class Tasks
    {
        [Key]
        public int Id { get; set; }

        public DateTime DateCreated { get; set; }

        [Required]
        [Display(Name = "What is your current status in relation to your goal?")]
        public string CurrentState { get; set; }

       
        [Display(Name = "What are your thoughts?")]
        public string Notes { get; set; }

        public int GoalId { get; set; }
        public Goals Goal { get; set; }
    }
}
