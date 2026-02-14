using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain
{
   public class LichSuChiNo:BaseEntity
    {
        public string MaPhieuNo { get; set; }
        public string NgayChi { get; set; }
        public string MaPhieuChi { get; set; }
        public string LoaiPhieuNo { get; set; }
        public double SoTienNo { get; set; }
        public double SoTienChi { get; set; }
    }
}
