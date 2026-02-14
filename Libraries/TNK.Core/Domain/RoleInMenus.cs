using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain
{
    public class RoleInMenus
    {
        public int id { get; set; }
        public string Name { get; set; }
        public int? mid { get; set; }
        public bool Active { get; set; }
    }
}
