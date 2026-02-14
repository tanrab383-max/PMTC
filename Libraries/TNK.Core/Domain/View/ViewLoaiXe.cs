using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain.View
{
    public class ViewLoaiXe:BaseEntity
    {
        public string MaLoaiXe { get; set; }
        public int MaMau { get; set; }
        public int MaModel { get; set; }
        public int Grad { get; set; }
        public int DoiXe { get; set; }

        public string TenMau { get; set; }
        public string TenModel { get; set; }
        public string TenGrad { get; set; }
        public string TenDoiXe { get; set; }

        public string CodeMau { get; set; }
        public string CodeModel { get; set; }
        public string CodeGrad { get; set; }
        public string CodeDoiXe { get; set; }
        public string Hang { get; set; }

        public Double? GiaNiemYet { get; set; }
        public string GhiChu { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        //public bool IsActive { get; set; }
        public string XuatXu { get; set; }
    }
}
