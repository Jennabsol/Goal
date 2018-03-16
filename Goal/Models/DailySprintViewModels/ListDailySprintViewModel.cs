using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Goal.Models.DailySprintViewModels
{
    public class ListDailySprintViewModel
    {
        public List<DailySprints> DailySprints { get; set; }
        public SprintGroup SprintGroup { get; set; }
    }
}
