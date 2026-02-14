using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain.View
{
    public class ViewTVAMU:BaseEntity
    {
        public string MaPhieuThu { get; set; }
        public DateTime NgayThu { get; set; }
        public string SoChungTu { get; set; }
        public string DVCV { get; set; }
        public string DienThoai { get; set; }
        public double TongCong { get; set; }
        public string TinhTrangPhieu { get; set; }
        public string NoiDung { get; set; }
        public double ConLai { get; set; }
        public double SoTienThu { get; set; }
    }
}
