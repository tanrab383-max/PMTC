using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain
{
    public class ChiTietNhapKhoPhuTung:BaseEntity
    {
        public new  Guid Id { get; set; }
        public string MaPhieuChi { get; set; }        
        public string MaKho { get; set; }
        public DateTime? NgayNhapKho { get; set; }
        public double SoTien { get; set; }
        public double SoLuong { get; set; }
        public string GhiChu { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
