using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain
{
    public class LoaiXe:BaseEntity
    {
        public string MaLoaiXe { get; set; }
        public int MaMau { get; set; }
        public int MaModel { get; set; }
        public int Grad { get; set; }
        public int DoiXe { get; set; }
        public string Hang { get; set; }
        public double? GiaNiemYet { get; set; }
        public string GhiChu { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string TenLoai { get; set; }
        public string XuatXu { get; set; }

    }
}
