using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain.View
{
    public class ViewUsers : BaseEntity
    {
        public System.Guid UserId { get; set; }
        public string UserName { get; set; }
    }
}
