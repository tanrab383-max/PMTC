using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace TNK.Core.Domain
{
    public class KhoPhuTung:BaseEntity
    {
        public string MaKho { get; set; }
        public string TenKho { get; set; }
        public string LoaiKho { get; set; }
        public string MaNCC { get; set; }
        public double? PhanTramGiaVon { get; set; }
        public double? SoTienKhoiTao { get; set; }
        public double? TongTien { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
}
