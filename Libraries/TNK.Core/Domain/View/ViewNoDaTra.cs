using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain.View
{
    public class ViewNoDaTra:BaseEntity 
    {
        public string MaPhieuNo { get; set; }
        public string MaPhieuChi { get; set; }
        public DateTime NgayNo { get; set; }
        public string LoaiPhieuNo { get; set; }
        public string LyDoNo { get; set; }
        public string GhiChu { get; set; }
        public string NoiDung { get; set; }
        public string ChungTuThu { get; set; }
        public string SoPhieuQuyetToan { get; set; }
        public string BienSo { get; set; }
        public string KhachHang { get; set; }
        public string DienThoai { get; set; }
        public string DiaChi { get; set; }
        public string SoKhung { get; set; }
        public string SoMay { get; set; }
        public string SoHopDong { get; set; }
        public DateTime? NgayHopDong { get; set; }
        public string NguoiBaoLanh { get; set; }
        public double SoTienThanhToan { get; set; }
        public double SoTienNo { get; set; }
      //  public double ConNo { get; set; }
        public double SoTienDaTra { get; set; }
        public double SoTienConLai { get; set; }
        public string ThongTinDonViNo { get; set; }
        public string TinhTrang { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public DateTime NgayChi { get; set; }
        public Guid? NguoiLapPhieu { get; set; }
        public Guid? NguoiNopTien { get; set; }
        public Guid? NguoiThuTien { get; set; }
        public Guid? KeToanTruong { get; set; }
        public string DoiTac { get; set; }
        public string NguoiDuyet { get; set; }
        public string Hoten { get; set; }
        public DateTime NgayHachToan { get; set; }
        public string MaPhieuLienQuan { get; set; }
        public string LyDoChi { get; set; }
    }
}
