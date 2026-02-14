using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain
{
    public class LichSuThaoTac : BaseEntity
    {
        public Guid? ID { get; set; }
        public string SessionId { get; set; }
        public string IPClient { get; set; }
        public string HostNameClient { get; set; }
        public string MaPhieu { get; set; }
        public string LoaiPhieu { get; set; }
        public string HanhDong { get; set; }
        public string GhiChu { get; set; }
        public string ChiTiet { get; set; }
        public double SoTienCanTaiKhoan { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
