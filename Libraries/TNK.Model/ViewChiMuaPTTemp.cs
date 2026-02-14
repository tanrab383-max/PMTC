using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core;

namespace TNK.Model
{
    public class ViewChiMuaPTTemp : BaseEntity
    {
        public string So_ct { get; set; }
        public DateTime Ngay_ct { get; set; }
        public string Dien_giai { get; set; }
        public double GiaBan { get; set; }
        public string NhaCungCap { get; set; }
        public string MaKho { get; set; }
        public double ThanhTien { get; set; }

        public string CyberId { get; set; }
        public string Ma_HS { get; set; }
    }
}
