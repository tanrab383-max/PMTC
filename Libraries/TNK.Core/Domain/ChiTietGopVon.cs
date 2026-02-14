using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain
{
    public class ChiTietGopVon :BaseEntity
    {
        public new Guid Id { get; set; }
        public string MaKhoiTao { get; set; }
        public string MaDoiTac { get; set; }
        public double SoTien { get; set; }
        public double? TyLe { get; set; }
        public double SoTienThucTe { get; set; }
        public double SoTienConLai { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public double SoTienDotTruoc { get; set; }
        public double TyLeDotTruoc { get; set; }
        public double? SoTienHienTai { get; set; }
        public string LoaiPhieu { get; set; }
    }
}
