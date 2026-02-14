using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain.View
{
    public class ViewTBAHI : BaseEntity
    {
        public string MaPhieuThu { get; set; }
        public DateTime NgayThu { get; set; }
        public string SoChungTu { get; set; }
        public string CTBH { get; set; }
        public double TongCong { get; set; }
        public double GiamGia { get; set; }
        public double GiaVon { get; set; }
        public string TinhTrangPhieu { get; set; }
        public string HoTen { get; set; }
        public string SoHopDong { get; set; }
        public string BienSo { get; set; }
        public string SoKhung { get; set; }
        public double? TienTraBH { get; set; }
        public double? HoaHong { get; set; }
        public string LoaiBaoHiem { get; set; }

    }
}
