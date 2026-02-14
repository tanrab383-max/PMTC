using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain.View
{
    public class ViewTBDTK : BaseEntity
    {
        public string MaPhieuThu { get; set; }
        public string SoHopDong { get; set; }
        public string MaLoaiPhieu { get; set; }
        public double GiamGia { get; set; }
        public double GiaBan { get; set; }
        public double SoTienThu { get; set; }
        public string TenChuongTrinh { get; set; }
        public bool IsDeleted { get; set; }

    }
}
