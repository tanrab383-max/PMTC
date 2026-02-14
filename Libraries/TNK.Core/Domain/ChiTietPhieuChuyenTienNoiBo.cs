using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain
{
    public class ChiTietPhieuChuyenTienNoiBo :BaseEntity
    {
        public new Guid Id { get; set; }
        public string MaPhieu { get; set; }
        public string TempChi { get; set; }
        public string TempThu { get; set; }
        public string TaiKhoanNguon { get; set; }
        public double SoTienThanhToan { get; set; }
        public string TaiKhoanDich { get; set; }
        public string HinhThucThanhToan { get; set; }
        public string GhiChu { get; set; }
        public string SoThamChieu { get; set; }
        public bool TinhTrang { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? NgayTreoTien { get; set; }
        public DateTime? NgayTienVao { get; set; }
    }
}
