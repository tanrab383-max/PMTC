using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain
{
    public class DoiTac:BaseEntity
    {

        [StringLength(30)]
        public string MaDoiTac { get; set; }
        public string MaVietTat { get; set; }
        public string TenDoiTac { get; set; }
        public string GhiChu { get; set; }
        public string LoaiDoiTac { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public double? HanMucVay { get; set; }
        public double? TyLeHoaHong { get; set; }
        public string DoiTacNoiBo { get; set; }
    }
}
