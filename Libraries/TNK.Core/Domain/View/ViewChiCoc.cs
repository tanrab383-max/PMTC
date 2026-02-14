using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain.View
{
    public class ViewChiCoc : BaseEntity
    {
        public string MaPhieuChi { get; set; }
        public string MaLoaiPhieu { get; set; }
        public string SoChungTu { get; set; }
        public DateTime NgayChi { get; set; }
        public string DoiTac { get; set; }
        public double SoTienChi { get; set; }
        public string GhiChu { get; set; }
        public double TongCong { get; set; }
        public string NguoiDuyet { get; set; }
        public double ConLai { get; set; }

    }
}
