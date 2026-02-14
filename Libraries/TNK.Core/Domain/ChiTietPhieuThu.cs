using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain
{
    public class ChiTietPhieuThu:BaseEntity
    {
        public new Guid Id { get; set; }
        public string MaPhieuThu { get; set; }
        public string HinhThucThanhToan { get; set; }
        public double SoTienThanhToan { get; set; }
        public string MaNganHang { get; set; }
        public string GhiChu { get; set; }
        public string SoThamChieu { get; set; }
        public bool TinhTrang { get; set; }
        public string Temp { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public DateTime? NgayTreoTien { get; set; }
        public DateTime? NgayTienVao { get; set; }
    }
}
