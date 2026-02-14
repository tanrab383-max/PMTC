using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain
{
    public class RoleInReports
    {
        public Guid RoleId { get; set; }
        public Guid ReportId { get; set; }
        public bool IsActive { get; set; }
    }
}
