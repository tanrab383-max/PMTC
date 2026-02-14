using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain
{
    public partial class DanhMucDienGiai : BaseEntity
    {
        public new Guid ID { get; set; }
        public string MaVietTat {get;set;}
        public string TableSuDung { get; set; }
        public string Ten { get; set; }
        public string DienGiai { get; set; }
        public Boolean IsActive { get; set; }
        public Boolean IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
}
