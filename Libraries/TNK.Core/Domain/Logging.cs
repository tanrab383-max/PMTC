using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain
{
    public class Logging:BaseEntity
    {
        public Guid?  ID { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ErrorMessage { get; set; }        
    }
}
