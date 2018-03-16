using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Goal.Models.SprintGroupViewModels
{
    public class ListSprintGroupViewModel
    {
        public List<SprintGroup> SprintGroup { get; set; }
        public List<DailySprints> DailySprints { get; set; }
        public Goals Goal { get; set; }
    }
}
