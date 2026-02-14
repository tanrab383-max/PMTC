using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain
{
    public class ChiTietBanTaiSan : BaseEntity
    {
        public new Guid Id { get; set; }
        public string MaPhieuThu { get; set; }
        public  Guid MaTaiSan { get; set; }
        public double GiaBan { get; set; }
        public string TinhTrang { get; set; }
        public bool IsDeleted { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
