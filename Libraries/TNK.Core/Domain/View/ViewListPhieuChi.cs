using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain.View
{
    public class ViewListPhieuChi : BaseEntity
    {
        public string MaPhieuChi { get; set; }
        public string MaLoaiPhieu { get; set; }
        public string SoHopDong { get; set; }
        public string SoChungTu { get; set; }
        public DateTime NgayChi { get; set; }
        public string Hoten { get; set; }
        public string NoiDung { get; set; }
        public double TongCong { get; set; }
        public double SoTienChi { get; set; }
        public string HinhThucThanhToan { get; set; }
        public double? ThanhToanNganHang { get; set; }
        public double? ThanhToanTienMat { get; set; }
        public bool IsActive { get; set; }
        public string DoiTac { get; set; }
        public string HoTenKH { get; set; }
        public string TenTui { get; set; }
        public double ThanhToanDungCoc { get; set; }
        public DateTime NgayHachToan { get; set; }
        public string LyDoChi { get; set; }
        public string GhiChu { get; set; }
        

    }
}
