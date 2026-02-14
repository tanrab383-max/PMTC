using System;

namespace TNK.Core.Domain
{
    public partial class RoleInMenuAction : BaseEntity
    {
        public Nullable<System.Guid> RoleId { get; set; }
        public Nullable<int> MenuActionId { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}
