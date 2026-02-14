using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain
{
    public class NoPhaiTra : BaseEntity
    {
        public string MaPhieuNo { get; set; }
        public string LoaiPhieuNo { get; set; }
        public string MaPhieuPhatSinh { get; set; }
        public double SoTienNo { get; set; }
        public double SoTienDaTra { get; set; }
        public double SoTienConLai { get; set; }
        public string NguoiBaoLanh { get; set; }
        public string NguoiPheDuyet { get; set; }
        public string ThongTinDoiTac { get; set; }
        public string MaPhieuChi { get; set; }
        public string DonViNo { get; set; }
        public string GhiChu { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
