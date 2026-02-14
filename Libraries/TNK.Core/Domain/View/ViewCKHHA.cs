using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain.View
{
    public class ViewCKHHA : BaseEntity
    {
        public string MaPhieuChi { get; set; }
        public DateTime NgayChi { get; set; }
        public DateTime NgayHachToan { get; set; }
        public string MaLoaiPhieu { get; set; }
        public string SoChungTu { get; set; }
        public string DuAn { get; set; }
        public string TenDuAn { get; set; }
        public double TongCong { get; set; }
        public string TaiSan { get; set; }
        public string NhomTaiSan { get; set; }
        public string TenTaiSan { get; set; }
        public string MaPhieuChiGoc { get; set; }
        public string NoiDung { get; set; }
    }
}
