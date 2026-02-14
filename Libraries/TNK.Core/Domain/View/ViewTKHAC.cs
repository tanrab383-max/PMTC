using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain.View
{
    public class ViewTKHAC : BaseEntity
    {
        public string MaPhieuThu { get; set; }
        public DateTime NgayThu { get; set; }
        public string SoChungTu { get; set; }
        public string ThongTinKhac { get; set; }
        public double TongCong { get; set; }
        public string TinhTrangPhieu { get; set; }
        public double SoTienThu { get; set; }
        public double ConLai { get; set; }
        public string TenDoiTac { get; set; }
    }
}
