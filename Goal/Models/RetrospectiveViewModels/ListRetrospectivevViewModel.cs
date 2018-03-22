using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Goal.Models.RetrospectiveViewModels
{
    public class ListRetrospectivevViewModel
    {
        public List<Retrospective> Retrospective { get; set; }
        public SprintGroup SprintGroup { get; set; }
    }
}
