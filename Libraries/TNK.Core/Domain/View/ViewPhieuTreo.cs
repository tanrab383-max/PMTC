using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain.View
{
    public class ViewPhieuTreo : BaseEntity
    {
        public new Guid Id { get; set; }
        public DateTime NgayThu { get; set; }
        public string MaPhieuThu { get; set; }
        public string HoTen { get; set; }
        public string MaNganHang { get; set; }
        public string TenNganHang { get; set; }
        public string HinhThucThanhToan { get; set; }
        public double SoTienThanhToan { get; set; }
        public string SoThamChieu { get; set; }
        public string SoChungTu { get; set; }
        public bool TinhTrang { get; set; }
        public DateTime? NgayTienVao { get; set; }
        public DateTime? NgayTreoTien { get; set; }
        public string DoiTac { get; set; }
        public string NhaCungCap { get; set; }
        public string MaLoaiPhieu { get; set; }
        public string TenDoiTac { get; set; }
    }
}
