using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain
{
    public class PhieuQuyetToan : BaseEntity
    {
        public string MaPhieu { get; set; }

        public string MaLoaiPhieu { get; set; }

        public string MaPhieuLienQuan { get; set; }

        public string SoChungTu { get; set; }

        public DateTime? NgayQuyetToan { get; set; }

        public double? SoTien { get; set; }

        public string SoHoaDon { get; set; }

        public string GhiChu { get; set; }

        public Guid? KeToanTruong { get; set; }

        public Guid? NguoiLapPhieu { get; set; }

        public Guid? NguoiNopTien { get; set; }

        public Guid? NguoiThuTien { get; set; }

        public double? TongCong { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsDeleted { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

    }

}
