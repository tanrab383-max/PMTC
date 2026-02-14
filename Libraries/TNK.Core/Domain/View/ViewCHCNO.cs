using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain.View
{
    public class ViewCHCNO : BaseEntity
    {
        public string MaPhieuChi { get; set; }
        public DateTime NgayChi { get; set; }
        public string SoChungTu { get; set; }
        public string TenDoiTac { get; set; }
        public double SoTienCoc { get; set; }
        public int DaSuDung { get; set; }
        public int ConLai { get; set; }
        public double SoTienChi { get; set; }
        public DateTime NgayHachToan { get; set; }
        public string NoiDung { get; set; }
        public string GhiChu { get; set; }
        public string LyDoChi { get; set; }

    }
}
