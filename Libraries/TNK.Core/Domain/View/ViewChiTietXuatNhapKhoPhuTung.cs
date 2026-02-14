using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain.View
{
    public class ViewChiTietXuatNhapKhoPhuTung : BaseEntity
    {
        public Guid Id { get; set; }
        public string MaPhieu { get; set; }
        public string MaKho { get; set; }
        public double SoTien { get; set; }
        public string TangGiam { get; set; }
        public string Loai { get; set; }
        public string MaPhuTung { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
