using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain.Report
{
    [Serializable]
    public class ReportCongNo:BaseEntity
    {
        public string MaPhieuThu { get; set; }
        public string HoTen { get; set; }
        public string SoHopDong { get; set; }
        public DateTime NgayThu { get; set; }
        public string LoaiXe { get; set; }
        public string SoKhung { get; set; }
        public string MaLoaiPhieu { get; set; }
        public string GhiChu { get; set; }
    }
}
