using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain
{
    public class ChiTietPhieuDichVu : BaseEntity
    {
        //public new int Id { get; set; }
        public string MaPhieuThu { get; set; }
        public string REPAIRORDERNO { get; set; }
        public string JOBSNAME { get; set; }
        public double QUANTITY { get; set; }
        public double COST { get; set; }
        public double DISCOUNT { get; set; }//do phai tinh 10% VAT
        public string ROTYPE { get; set; }
        public string KPI_PART_TYPE { get; set; }
        public double GCN { get; set; }
        public string WTCODE { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string MaKho { get; set; }
        public string PhanLoai { get; set; }
        public double? GiaVon { get; set; }
        public double? GiaVonVAT { get; set; }
        public double VAT { get; set; }
    }
}
