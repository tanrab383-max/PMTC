using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain
{
    public partial class Category : BaseEntity
    {
        public string Code { get; set; }
        public int Parent { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public int LastCode { get; set; }
        public bool IsDeleted { get; set; }
        [Display(Name = "Active")]
        public bool IsActive { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool Children { get; set; }
    }
}
