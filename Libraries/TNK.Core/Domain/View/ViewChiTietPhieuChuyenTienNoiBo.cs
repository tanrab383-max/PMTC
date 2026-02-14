using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain.View
{
    public class ViewChiTietPhieuChuyenTienNoiBo:BaseEntity
    {
        public new Guid Id { get; set; }
        public string MaPhieu { get; set; }
        public string TaiKhoanNguon { get; set; }
        public string TaiKhoanDich { get; set; }
        public double SoTienThanhToan { get; set; }
        public string GhiChu { get; set; }
        public string SoThamChieu { get; set; }
        public bool TinhTrang { get; set; }
        public DateTime NgayChuyen { get; set; }
        public string SoChungTu { get; set; }
        public string NoiDung { get; set; }
    }
}
