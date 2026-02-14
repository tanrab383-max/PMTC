using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain
{
    public class DieuChinhKhoPhuTung :BaseEntity
    {
        public DieuChinhKhoPhuTung()
        {
            NgayDieuChinh = DateTime.Now;
            ID = Guid.Empty;
            MaKho = "";
            SoTienDieuChinh = 0;
            IsActive = true;
            IsDeleted = false;
            CreatedBy = Guid.Empty;
            CreatedDate = DateTime.Now;
            UpdatedBy = Guid.Empty;
            UpdatedDate = DateTime.Now;
        }
        public Guid ID { get; set; }
        public DateTime NgayDieuChinh { get; set; }
        public string MaKho { get; set; }
        public double SoTienDieuChinh { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
