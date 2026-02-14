using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain.View
{
    public class ViewCTAUN : BaseEntity
    {
        public ViewCTAUN()
        {
            MaPhieuChi = "";
            SoChungTu = "";
            NgayChi = DateTime.Now;
            NgayHachToan = DateTime.Now;
            NgayQuyetToan = DateTime.Now;
            HoTen = "";
            DienThoai = "";
            LyDoChi = "";       
            KeToanTruong = Guid.Empty;
            NguoiLapPhieu = Guid.Empty;
            NguoiLapPhieu = Guid.Empty;
            NguoiThuTien = Guid.Empty;
            TongCong = 0;
            //TinhTrang = "";
            NoiDung = "";
            GhiChu = "";
        }
        public string MaPhieuChi { get; set; }
      
        public string SoChungTu { get; set; }
        public DateTime NgayChi { get; set; }
        public DateTime NgayHachToan { get; set; }
        public DateTime? NgayQuyetToan { get; set; }      
        public string HoTen { get; set; }
        public string DienThoai { get; set; }     
        public Guid? KeToanTruong { get; set; }
        public Guid? NguoiLapPhieu { get; set; }
        public Guid? NguoiNopTien { get; set; }
        public Guid NguoiThuTien { get; set; }
        public string NgayHoanUng { get; set; }
        public double TongCong { get; set; }
        public double SoTienDaTra { get; set; }
        public double SoTienConLai { get; set; }
        public string NguoiUngTien { get; set; }
        public string NguoiDuyet { get; set; }
        public string TinhTrang { get; set; }
        public string NoiDung { get; set; }
        public string LyDoChi { get; set; }
        public string GhiChu { get; set; }
    }
}
