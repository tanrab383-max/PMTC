using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain.View
{
    public class View_DSDieuChinhKhoPhuTung :BaseEntity
    {
        public string NgayDieuChinh { get; set; }
        public double SoTienDieuChinh { get; set; }
        public string MaKho { get; set; }
        public string NguoiDieuChinh { get; set; }
    }
}
