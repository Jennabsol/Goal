using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Goal.Models
{
    public class Tips
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Tip to help goal!")]
        public string Name { get; set; }
    }
}
