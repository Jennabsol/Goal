using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Goal.Models.RetrospectiveViewModels
{
    public class EditRetrospectivevViewModel
    {
        public Retrospective Retrospective { get; set; }
        public int SprintGroupId { get; set; }
    }
}
