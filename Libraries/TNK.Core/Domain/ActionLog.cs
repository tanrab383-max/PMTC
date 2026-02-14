using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain
{
    public class ActionLog : BaseEntity
    {
        public Guid ID { get; set; }
        public string UserName { get; set; }
        public string Url { get; set; }
        public string Action { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public double Duration { get; set; }
        public string Error { get; set; }
        public DateTime ActionTime { get; set; }
    }
}
