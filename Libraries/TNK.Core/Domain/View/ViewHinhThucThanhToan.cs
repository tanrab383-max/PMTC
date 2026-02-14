using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain.View
{
    public class ViewHinhThucThanhToan:BaseEntity
    {
        public string MaNganHang { get; set; }
        public string MaHinhThuc { get; set; }
        public string GiaTri { get; set; }
        public string HienThi { get; set; }
    }
}
