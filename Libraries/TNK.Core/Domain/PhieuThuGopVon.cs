using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain
{
    public class PhieuThuGopVon : BaseEntity
    {
        public string SoChungTu { get; set; }
        public DateTime NgayThu { get; set; }
        public float TongCong { get; set; }
        public string GhiChu { get; set; }
        public string TenDoiTac { get; set; }
        public string MaDoiTac { get; set; }
    }
}
