using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core;

namespace TNK.Model
{
    public class ViewChiTietBDTK:BaseEntity
    {
        public string MaBDTK { get; set; }
        public string TenBDTK { get; set; }
        public double GiaNiemYet { get; set; }
        public double GiaBan { get; set; }
        public double GiamGia { get; set; }
        public double ThucBan { get; set; }
        public DateTime? HanSuDung { get; set; }
        public string MaPhieuSuDung { get; set; }
        public string Id_Cyber { get; set; }
    }
}
