using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain.View
{
    public class ViewNguonKhuyenMai:BaseEntity
    {
        public string NguonKhuyenMai { get; set; }
        public string TenKhuyenMai { get; set; }
        public string LoaiKhuyenMai { get; set; }
        public double SoTienKhoiTao { get; set; }
        public double SoTienDangCo { get; set; }
    }
}
