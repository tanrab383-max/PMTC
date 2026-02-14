using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain
{
    public class DotGopVon : BaseEntity
    {
        public string MaKhoiTao { get; set; }
        public DateTime NgayGopVon { get; set; }
        public string TenKhoiTao { get; set; }
        public string LoaiGopVon { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
