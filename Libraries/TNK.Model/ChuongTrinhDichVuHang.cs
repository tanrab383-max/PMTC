using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core;

namespace TNK.Model
{
    public class ChuongTrinhDichVuHang:BaseEntity
    {
        public string MaCTDV { get; set; }

        public string TenCTDV { get; set; }

        public string GhiChu { get; set; }

        public string PhanLoai { get; set; }

        public string MaDoiTac { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsDeleted { get; set; }

        public Guid? CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

    }
}
