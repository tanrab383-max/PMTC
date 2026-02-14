using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TNK.Core.Domain.View
{
    public class ViewNoPhaiThu : BaseEntity
    {
        public string MaPhieuNo { get; set; }
        public DateTime NgayNo { get; set; }
        public string LoaiPhieuNo { get; set; }
        public string LyDoNo { get; set; }
        public string GhiChu { get; set; }
        public string ChungTuThu { get; set; }
        public string SoPhieuQuyetToan { get; set; }
        public string BienSo { get; set; }
        public string KhachHang { get; set; }
        public string DienThoai { get; set; }
        public string DiaChi { get; set; }
        public string SoKhung { get; set; }
        public string SoMay { get; set; }
        public string SoHopDong { get; set; }
        public DateTime NgayHopDong { get; set; }
        public string NguoiBaoLanh { get; set; }
        public double SoTienNo { get; set; }
        public double SoTienDaTra { get; set; }
        public double SoTienConLai { get; set; }
        public string ThongTinDonViNo { get; set; }
        public string TinhTrang { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string MaPhieuPhatSinh { get; set; }
        public string DonViNo { get; set; }
        public DateTime? NgayHachToan { get; set; }
        public DateTime NgayThu { get; set; }
        public string NoiDung {get;set;}
        public string Hoten { get; set;}
        public string Sales { get; set; }
        public string NguoiVay { get; set; }
        public string HoTenKH { get; set; }
        public string TenDoiTac { get; set; }
        public string MaPhieuThu { get; set; }
        public string LyDoChi { get; set; }
        public string ThongTinKhac { get; set; }
        public string SoHoaDon { get; set; }
        public string SoQuyetToanDV { get; set; }
        public string LoaiNo { get; set; }
    }
}
