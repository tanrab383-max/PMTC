using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace TNK.Core.Domain
{
    public class KhoTaiSan:BaseEntity
    {
        public Guid Id { get; set; }
        public string TenTaiSan { get; set; }
        public DateTime? NgayNhapKho { get; set; }
        public int? ThoiGianKhauHao { get; set; }
        public string MaLoaiTS { get; set; }
        public string MaPhieuNhap { get; set; }
        public string MaPhieuXuat { get; set; }
        public string NhaCungCap { get; set; }
        public double? GiaMua { get; set; }
        public string HinhThucKhauHao { get; set; }
        public double? GiaTriConLai { get; set; }
        public string TinhTrang { get; set; }
        public string GhiChu { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
}
