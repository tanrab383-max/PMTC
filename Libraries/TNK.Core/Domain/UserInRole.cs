using System;

namespace TNK.Core.Domain
{
    public partial class UserInRole :BaseEntity
    {
        public System.Guid RoleId { get; set; }
        public System.Guid UserId { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
    }
}
