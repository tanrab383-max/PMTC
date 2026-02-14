using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain.View
{
    public class ViewGiaoDichTuiTien:BaseEntity
    {
        public Guid Id { get; set; }
        public Guid IdChiTietPhieu { get; set; }
        public string LoaiThuChi { get; set; }
        public string MaLoaiPhieu { get; set; }
        public string MaTuiTien { get; set; }
        public string MaPhieuPhatSinh { get; set; }
        public string HinhThucThanhToan { get; set; }
        public double SoTien { get; set; }
        public int TangGiam { get; set; }
        public string GhiChu { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime? NgayPhatSinh { get; set; }
    }
}
