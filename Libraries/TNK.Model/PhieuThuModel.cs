using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core.Domain;
using TNK.Core.Domain.View;

namespace TNK.Model
{
    public class PhieuThuModel
    {
        public string MaPhieuThu { get; set; }
        public string SoChungTu { get; set; }
        public DateTime NgayThu { get; set; }
        public string Hoten { get; set; }
        public string DienThoai { get; set; }
        public string DiaChi { get; set; }
        public string BienSo { get; set; }
        public string MaXe { get; set; }
        public string MauXe { get; set; }
        public string NoiDung { get; set; }
        public string GhiChu { get; set; }
        public Guid KeToanTruong { get; set; }
        public Guid NguoiLapPhieu { get; set; }
        public Guid NguoiNopTien { get; set; }
        public Guid NguoiThuTien { get; set; }
    }
}
