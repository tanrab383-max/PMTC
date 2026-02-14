using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain.View
{
    public class ViewThongKeTien : BaseEntity
    {
        public string TenTui { get; set; }
        public string LoaiTui { get; set; }
        public string MaTuiCha { get; set; }
        public double SoTienDangCo { get; set; }
    }
}
