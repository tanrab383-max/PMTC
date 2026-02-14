using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain.View
{
    public class ViewChiTietKhauHao : BaseEntity
    {
        public Guid MaId { get; set; }
        public string MaPhieuChiKhauHao { get; set; }
        public string MaPhieuChiGoc { get; set; }
        public double SoTienConLai { get; set; }
        public double SoTienKhauHao { get; set; }
        public double ConLaiSauKhauHao { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string MaLoaiPhieu { get; set; }
        public string NoiDung { get; set; }
        public string TenTaiSan { get; set; }
    }
}
