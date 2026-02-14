using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain.View
{
    public class ViewCongNo : BaseEntity
    {
        public string MaPhieuThu { get; set; }
        //public string SoThamChieu { get; set; }
        public double TongCong { get; set; }
        public double SoTienThanhToan { get; set; }
        public string DoiTac { get; set; }
        public double ConLai { get; set; }
        public DateTime NgayThu { get; set; }
    }
}
