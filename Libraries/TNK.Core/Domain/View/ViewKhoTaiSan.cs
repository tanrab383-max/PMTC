using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain.View
{
    public class ViewKhoTaiSan : BaseEntity
    {
        public  Guid KId { get; set; }
        public string TenTaiSan { get; set; }
        public DateTime NgayNhapKho { get; set; }
        public int ThoiGianKhauHao { get; set; }
        public string MaLoaiTS { get; set; }
        public string TenLoaiTS { get; set; }
        public string MaPhieuNhap { get; set; }
        public string MaPhieuXuat { get; set; }
        public string NhaCungCap { get; set; }
        public double GiaMua { get; set; }
        public string HinhThucKhauHao { get; set; }
        public double TienKhauHao { get; set; }
        public double GiaTriConLai { get; set; }
        public string TinhTrang { get; set; }
        public string GhiChu { get; set; }
        public double GiaBan { get; set; }
        public DateTime NgayThu { get; set; }
    }
}
