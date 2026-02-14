using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain
{
    public class LichSuTheChapXe : BaseEntity
    {
        public new Guid Id { get; set; }
        public string SoKhung { get; set; }
        public DateTime NgayTheChap { get; set;}
        public DateTime? NgayHetTheChap { get; set; }
        public string NganHangTheChap { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
