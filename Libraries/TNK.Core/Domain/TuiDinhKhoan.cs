using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain
{
    public class TuiDinhKhoan : BaseEntity
    {
        public string MaTui { get; set; }
        public string TenTui { get; set; }
        public string LoaiTui { get; set; }
        public string MaTuiCha { get; set; }
        public double? SoTienKhoiTao { get; set; }
        public double? SoTienThayDoi { get; set; }
        public double? SoTienDangCoLanTruoc { get; set; }
        public double? SoTienDangCo { get; set; }
        public string GhiChu { get; set; }
        public string LastSessionId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
