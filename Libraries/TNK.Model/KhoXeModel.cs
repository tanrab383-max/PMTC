using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TNK.Core.Domain;
using TNK.Core.Domain.View;

namespace TNK.Model
{
    public class KhoXeModel
    {
        public List<DoiTac> ListNhaCungCap { get; set; }
        public KhoXe Item { get; set; }
        public ViewTuiHangTrenDuong TuiHangTrenDuong { get; set; }
        public PhieuNhapKho PhieuNhapKho { get; set; }
        public List<ViewLoaiXe> MaXe { get; set; }
        public LichSuTheChapXe TheChap { get; set; }
    }
}
