using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Goal.Models
{
    public class DailySprints
    {
        [Key]
        public int Id { get; set; }

        public DateTime DateCreated { get; set; }

        [Required]
        [Display(Name = "What is your current status in relation to your goal?")]
        public string CurrentState { get; set; }

       
        [Display(Name = "What are your thoughts?")]
        public string Notes { get; set; }

        public int SprintGroupId { get; set; }
        public SprintGroup SprintGroup { get; set; }
    }
}
