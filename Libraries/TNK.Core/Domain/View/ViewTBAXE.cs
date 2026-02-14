using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain.View
{
    public class ViewTBAXE:BaseEntity
    {
        public string SoHopDong { get; set; }
        public DateTime NgayHopDong { get; set; }
        public string HoTen { get; set; }
        public string DiaChi { get; set; }
        public string DienThoai { get; set; }
        public string SoKhung { get; set; }
        public string SoMay { get; set;}
        public string LoaiXe { get; set; }
        public double GiaNiemYet { get; set; }
        public double GiamGia { get; set; }
        public double? GiaBan { get; set; }
        public string MauXe { get; set; }
        public string TinhTrang { get; set; }
        public string DoiTac { get; set; }
        public double? HoaHong { get; set; }
        public double? PhiTruocBa { get; set; }
        public string NhanVienTuVan { get; set; }
        public double? PhuKienHuaTang { get; set; }
    }
}
