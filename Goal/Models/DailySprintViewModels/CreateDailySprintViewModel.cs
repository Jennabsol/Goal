using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Goal.Models.DailySprintViewModels
{
    public class CreateDailySprintViewModel
    {
        public DailySprints DailySprints { get; set; }
        public int SprintGroupId { get; set; }
    }
}
