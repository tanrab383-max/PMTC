using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain
{
    public class LichSuThayDoiTuiDinhKhoan :BaseEntity
    {
        public string SessionId { get; set; }
        public string MaTui { get; set; }
        public double SoTienTruocThayDoi { get; set; }
        public double SoTienSauThayDoi { get; set; }
    }
}
