using System;
using System.ComponentModel.DataAnnotations;

namespace TNK.Core.Domain
{
    public partial class Menu : BaseEntity
    {
        public int Parent { get; set; }
        public string Icon { get; set; }
        [Required(ErrorMessage ="Bắt buộc nhập")]
        public string Name { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public int OrderBy { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.Guid> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.Guid> UpdatedBy { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string ActionDefault { get; set; }
        //public string ActionKey { get; set; }
    }
}
