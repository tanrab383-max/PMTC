using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain.View
{
    public class ViewChiTietPhuKien : BaseEntity
    {
        public string MaKho { get; set; }
        public decimal GiaBan { get; set; }
        public decimal GiamGia { get; set; }
        public decimal ThucBan { get; set; }
        public decimal GiaVon { get; set; }
        public decimal SoLuong { get; set; }
        public decimal TienCong { get; set; }
    }
}
