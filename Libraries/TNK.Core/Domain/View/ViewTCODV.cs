using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain.View
{
    public class ViewTCODV:BaseEntity
    {
        public string MaPhieuThu { get; set; }
        public DateTime NgayThu { get; set; }
        public string SoChungTu { get; set; }
        public string BienSo { get; set; }
        public string HoTen { get; set; }
        public string MaXe { get; set; }
        public string MauXe { get; set; }
        public string DienThoai { get; set; }
        public string DiaChi { get; set; }
        public double TongCong { get; set; }
        public string TinhTrangPhieu { get; set; }
        public double SoTienThu { get; set; }
        public double? SoTienDaSuDung { get; set; }
        public double ConLai { get; set; }
        public double? SoTienChiTra { get; set; }
        public string TTPCode { get; set; }
        public string GhiChu { get; set; }
    }
}
