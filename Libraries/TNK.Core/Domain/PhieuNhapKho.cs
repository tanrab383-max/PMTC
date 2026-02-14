using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain
{
    public class PhieuNhapKho : BaseEntity
    {
        public string MaPN { get; set; }
        public DateTime NgayNhap { get; set; }
        public string NguoiNhap { get; set; }
        public string GhiChu { get; set; }
        public string PhanLoaiNhap { get; set; }
        public string MaPhieuChi { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public double SoTienHTD { get; set; }
        public double SoTienKMTMV { get; set; }
        public double HoaHongHB { get; set; }
    }
}
