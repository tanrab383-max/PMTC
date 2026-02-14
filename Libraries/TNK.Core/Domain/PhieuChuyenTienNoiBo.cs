using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain
{
    public class PhieuChuyenTienNoiBo:BaseEntity
    {
        public string MaPhieu { get; set; }
        public string MaLoaiPhieu { get; set; }
        public string SoChungTu { get; set; }
        public DateTime NgayChuyen { get; set; }
        public DateTime NgayHachToan { get; set; }
        public string NoiDung { get; set; }
        public string GhiChu { get; set; }
        public Guid KeToanTruong { get; set; }
        public Guid NguoiLapPhieu { get; set; }
        public Guid NguoiNopTien { get; set; }
        public Guid NguoiThuTien { get; set; }
        public string TinhTrangPhieu { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
