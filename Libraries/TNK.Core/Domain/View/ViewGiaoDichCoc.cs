using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain.View
{
    public class ViewGiaoDichCoc : BaseEntity
    {
        public Guid ID { get; set; }
        public Guid IDChiTietPhieu { get; set; }
        public string LoaiThuChi { get; set; }
        public string MaLoaiPhieu { get; set; }
        public DateTime NgayPhatSinh { get; set; }
        public string MaPhieuLienQuan { get; set; }
        public string NoiDung { get; set; }
        public string MaTuiTien { get; set; }
        public string MaPhieuPhatSinh { get; set; }
        public string HinhThucThanhToan { get; set; }
        public double SoTien { get; set; }
        public int TangGiam { get; set; }
        public string DoiTac { get; set; }
        public string GhiChu { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string LyDo { get; set; }
        public string NguoiDuyet { get; set; }
        public string SoThamChieu { get; set; }
        public bool TinhTrang { get; set; }
        public DateTime? NgayTreoTien { get; set; }
        public DateTime? NgayTienVao { get; set; }
    }
}
