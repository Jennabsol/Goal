﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Goal.Models
{
    public class Goals
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "What is your goal!")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "What is you current status in relation to your goal?")]
        public string BeginningState { get; set; }


        public DateTime DateCreated { get; set; }

        [Required]
        public ApplicationUser User { get; set; }

    }
}