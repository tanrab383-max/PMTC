using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain.View
{
    public class ViewTDIVU:BaseEntity
    {
        public string MaPhieuThu{get;set;}
        public string SoHopDong { get;set;}
        public string BienSo { get;set;}
        public string SoKhung { get;set;}
        public string HoTen { get;set;}
        public string MaXe { get;set;}
        public string DienThoai { get; set; }
        public string MaLoaiPhieu { get; set; }
        public string SoThamChieu { get; set; }
        public DateTime NgayThu { get; set; }
        public double TongCong { get; set; }
        public double ConLai { get; set; }
        public double SoTienThu { get; set; }
        public bool? Flag { get; set; }
        public string Ext1 { get; set; }
        public string Ext2 { get; set; }
        public string Ext3 { get; set; }
    }
}
