using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain.View
{
    public class ViewNKMPK : BaseEntity
    {
        public string MaPhieuThu { get; set; }
        public DateTime NgayThu { get; set; }
        public string SoChungTu { get; set; }
        //public string LoaiXe { get; set; }
        //public string NhaCungCap { get; set; }
        public double TongCong { get; set; }
        public string TinhTrangPhieu { get; set; }
        public string HoTen { get; set; }
        public string DienThoai { get; set; }
        public double GiaBan { get; set; }
        public double GiamGia { get; set; }
        public double ConLai { get; set; }
        public double TienCongPK { get; set; }
        public double GiaVonKM { get; set; }
        public double TongKM { get; set; }
        public double SoTienThu { get; set; }
        public string BienSo { get; set; }
        public string SoHopDong { get; set; }
        public string SoKhung { get; set; }
    }
}
