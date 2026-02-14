using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain
{
    public class MenuRole
    {
        public int Id { get; set; }
        public Nullable<int> Parent { get; set; }
        public string Icon { get; set; }
        public string Name { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public string ActionDefault { get; set; }
        public int OrderBy { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> total { get; set; }
    }
}
