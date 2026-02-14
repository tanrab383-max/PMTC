using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain.View
{
    public class ViewTHHHB:BaseEntity
    {
        public string MaPhieuThu { get; set; }
        public DateTime NgayThu { get; set; }
        public string SoChungTu { get; set; }
        public string DoiTac { get; set; }
        public double TongCong { get; set; }
        public string TinhTrangPhieu { get; set; }
    }
}
