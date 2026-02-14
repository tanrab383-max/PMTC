using System;

namespace TNK.Core.Domain
{
    public partial class RoleInMenu : BaseEntity
    {
        public Nullable<System.Guid> RoleId { get; set; }
        public Nullable<int> MenuId { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<bool> IsActive { get; set; }
    }
}
