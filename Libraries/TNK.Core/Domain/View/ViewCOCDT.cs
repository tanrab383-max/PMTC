using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain.View
{
    public class ViewCOCDT : BaseEntity
    {
        public string MaPhieuChi { get; set; }
        public DateTime NgayChi { get; set; }
        public string SoChungTu { get; set; }
        public string SoHopDong { get; set; }
        public string HoTen { get; set; }
        public string DienThoai { get; set; }
        public double SoTienCoc { get; set; }
        public double SoTienChi { get; set; }
        public double ConLai { get; set; }
        public string MaXe { get; set; }
        public string MauXe { get; set; }
        public string MaPhieuLienQuan { get; set; }
        public DateTime NgayHachToan { get; set; }
        public string GhiChu { get; set; }
        public string LyDoChi { get; set; }
        public string NoiDung { get; set; }
    }
}
