using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain.View
{
    public class ViewThongKeKho : BaseEntity
    {
        public string MaLoaiXe { get; set; }
        public Int32? SoLuong { get; set; }
        public string TenLoai { get; set; }
        public double GiaVon { get; set; }
        public double SoTien { get; set; }
    }
}
