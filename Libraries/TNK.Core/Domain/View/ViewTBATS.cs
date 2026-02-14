using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain.View
{
    public class ViewTBATS : BaseEntity
    {
        public string MaPhieuThu { get; set; }
        public DateTime NgayThu { get; set; }
        public string SoChungTu { get; set; }
        public string HoTen { get; set; }
        public string DienThoai { get; set; }
        public double TongCong { get; set; }
        public double SoTienThu { get; set; }
        public double ConLai { get; set; }
        public string TenTS { get; set; }

    }
}
