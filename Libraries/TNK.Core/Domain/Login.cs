using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain
{
    public class Login : BaseEntity
    {
        public Guid? ID { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Active { get; set; }
        public string IPClient { get; set; }
        public string HostNameClient { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public string UserName { get; set; }
    }
}
